using AutoMapper;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;

using SistemaVenta.BLL.Servicios.Contrato;

using SistemaVenta.DTO;


namespace SistemaVenta.BLL.Servicios
{
    public  class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _categoriaRepositorio;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoriaRepositorio, IMapper mapper)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try
            {
                var listaCategorias = await _categoriaRepositorio.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listaCategorias);
            }
            catch
            {
                throw;
            }
        }
    }
}
