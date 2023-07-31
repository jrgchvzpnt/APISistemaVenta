using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository :GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbventaContext _dbcontext;

        public VentaRepository(DbventaContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Venta> Registrar(Venta Modelo)
        {
            Venta ventaGenerada = new Venta();

            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta dv in Modelo.DetalleVenta)
                    {
                        Producto producto_encontrado = _dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad;
                        _dbcontext.Productos.Update(producto_encontrado);
                    }
                    await _dbcontext.SaveChangesAsync();

                    NumeroDocumento correlativo = _dbcontext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;

                    _dbcontext.NumeroDocumentos.Update(correlativo);
                    await _dbcontext.SaveChangesAsync();


                    int CantidaDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidaDigitos));
                    string numeroVenta = ceros + correlativo.ToString();
                    //0001

                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - CantidaDigitos);

                    Modelo.NumeroDocumento = numeroVenta;

                    await _dbcontext.Venta.AddAsync(Modelo);
                    await _dbcontext.SaveChangesAsync();

                    ventaGenerada = Modelo;

                    transaction.Commit();

                }
                catch
                {
                    transaction.Rollback();
                    throw;

                }
                return ventaGenerada;
            }

                

            
        }
    }

}
