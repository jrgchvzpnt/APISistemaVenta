using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaServicio;

        public VentaController(IVentaService ventaServicio)
        {
            _ventaServicio = ventaServicio;
        }

        [HttpPost]
        [Route("Regitrar")]
        public async Task<IActionResult> Regitrar([FromBody] VentaDTO venta)
        {
            var rsp = new Response<VentaDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _ventaServicio.Registrar(venta);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }

        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string buscarPor, string? numeroVenta, string? fechainicio, string? fechafin)
        {
            var rsp = new Response<List<VentaDTO>>();
            numeroVenta = numeroVenta is null ? "" : numeroVenta;
            fechainicio = fechainicio is null ? "" : fechainicio;
            fechafin = fechafin is null ? "" : fechafin;


            try
            {
                rsp.status = true;
                rsp.value = await _ventaServicio.Historial(buscarPor, numeroVenta, fechainicio, fechafin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }

        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string? fechainicio, string? fechafin)
        {
            var rsp = new Response<List<ReporteDTO>>();
           


            try
            {
                rsp.status = true;
                rsp.value = await _ventaServicio.Reporte(fechainicio, fechafin);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }
    }
}
