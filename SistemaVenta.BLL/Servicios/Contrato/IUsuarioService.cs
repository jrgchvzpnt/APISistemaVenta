using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DTO;


namespace SistemaVenta.BLL.Servicios.Contrato
{
    public  interface IUsuarioService
    {
        Task<List<usuarioDTO>> Lista();

        Task<SessionDTO> ValidarCredenciales(string correo, string clave);

        Task<usuarioDTO> Crear(usuarioDTO Modelo);

        Task<bool> Editar(usuarioDTO Modelo);

        Task<bool> Eliminar(int id);

    }
}
