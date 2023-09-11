using AutoMapper;
using TPFinalBitwise.DTO;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.Utilidades
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            /*
            CreateMap<Venta, VentaDTO>().ForMember(d => d.FechaRealizacion,
                opt => opt.MapFrom(o => o.FechaRealizacion.ToString("dd/MM/yyyy")));

            CreateMap<VentaCreacionDTO, Venta>().ForMember(d => d.FechaRealizacion,
                opt => opt.MapFrom(o => DateTime.Parse(o.FechaRealizacion)));
            */

            CreateMap<Producto, ProductoDTO>()
                .ForMember(d => d.NombreCategoria, opt => opt.MapFrom(o => o.Categoria.Nombre));
            CreateMap<ProductoCreacionDTO, Producto>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Categoria, o => o.Ignore());
            CreateMap<ProductoRespuestaDTO, Producto>().ReverseMap();
            

            CreateMap<Categoria, CategoriaCreacionDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();           
            CreateMap<Categoria, CategoriaConProductosDTO>()
                .ForMember(d => d.Productos, opt => opt.MapFrom(o => o.Productos));

            CreateMap<Item, ItemCreacionDTO>().ReverseMap();
            CreateMap<Item, ItemDTO>().ReverseMap();
        }
    }
}
