using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;



namespace SistemaVenta.BLL.Servicios
{
    public  class ProductoService :IProductoService
    {
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IMapper _mapper;

        public ProductoService(IGenericRepository<Producto> productoRepositorio, IMapper mapper)
        {
            _productoRepositorio = productoRepositorio;
            _mapper = mapper;
        }
        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var queryproducto = await _productoRepositorio.Consultar();
                var listaProducto = queryproducto.Include(cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<ProductoDTO>>(listaProducto.ToList());   
            }
            catch
            {
                throw;
            }
        }
        public async Task<ProductoDTO> Crear(ProductoDTO Modelo)
        {
            try
            {
                var productoCreado = await _productoRepositorio.Crear(_mapper.Map<Producto>(Modelo));

                if (productoCreado.IdProducto == 0)
                    throw new TaskCanceledException("no se pudo crear");

                return _mapper.Map<ProductoDTO>(productoCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO Modelo)
        {
            try
            {
                var productoModelo = _mapper.Map<Producto>(Modelo);

                var productoEncontrado = await _productoRepositorio.Obtener(u =>
                     u.IdProducto == productoModelo.IdProducto
                    );

                if (productoEncontrado == null)
                    throw new TaskCanceledException("el producto no existe");

                productoEncontrado.Nombre = productoModelo.Nombre;
                productoEncontrado.IdCategoria = productoEncontrado.IdCategoria;
                productoEncontrado.Stock = productoModelo.Stock;
                productoEncontrado.Precio = productoModelo.Precio;
                productoEncontrado.EsActivo = productoModelo.EsActivo;

                bool respuesta = await _productoRepositorio.Editar(productoEncontrado);

                if (respuesta)
                    throw new TaskCanceledException("no se pudo editar");


                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var productoEncontrado = await _productoRepositorio.Obtener(p => p.IdProducto == id);

                if (productoEncontrado == null)
                    throw new TaskCanceledException("el producto no existe");

                bool respuesta = await _productoRepositorio.Eliminar(productoEncontrado);
                
                if (respuesta)
                    throw new TaskCanceledException("no se pudo elminar");

                return respuesta;

            }
            catch
            {
                throw;
            }
        }

       
    }
}
