using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TPFinalBitwise.DTO;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.Utilidades
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
              
            CreateMap<Producto, ProductoDTO>()
                .ForMember(d => d.NombreCategoria, opt => opt.MapFrom(o => o.Categoria.Nombre));
            CreateMap<ProductoCreacionDTO, Producto>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Categoria, o => o.Ignore());
            CreateMap<ProductoRespuestaDTO, Producto>().ReverseMap();
            //CreateMap<ProductoRespuestaDTO, Producto>().ForMember(d => d.Nombre, opt => opt.MapFrom(o => o.Nombre));

            CreateMap<Categoria, CategoriaCreacionDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();           
            CreateMap<Categoria, CategoriaConProductosDTO>()
                .ForMember(d => d.Productos, opt => opt.MapFrom(o => o.Productos));

            CreateMap<Item, ItemCreacionDTO>().ReverseMap();
            CreateMap<ItemDTO, ItemCreacionDTO>().ReverseMap();
            //CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<Item, ItemDTO>().ForMember(d => d.TotalItem, opt => opt.MapFrom(o => (o.Producto.Precio * o.Cantidad)));
            CreateMap<ItemDTO, Item>();
            //CreateMap<Item, ItemDatosVentaDTO>();
            CreateMap<Item, ItemDatosVentaDTO>().ForMember(d => d.TotalItem, opt => opt.MapFrom(o => (o.Producto.Precio * o.Cantidad)));
            CreateMap<ItemDatosVentaDTO, Item>();

            CreateMap<Venta, VentaDTO>().ForMember(d => d.FechaRealizacion,
                opt => opt.MapFrom(o => o.FechaRealizacion.ToString("dd/MM/yyyy")));
            CreateMap<VentaDTO, Venta>().ForMember(d => d.Items, opt => opt.MapFrom(o => o.Items));
            //    .ForPath(d => d.Usuario.Id, opt => opt.MapFrom(o => o.UserId));
            CreateMap<VentaCreacionDTO, Venta>().ForMember(d => d.FechaRealizacion,
                opt => opt.MapFrom(o => DateTime.Parse(o.FechaRealizacion))).ReverseMap();


            //var user = await UserManager.FindByIdAsync()
            //CreateMap<Usuario, UsuarioDatosDTO>().ForMember(d => d.Role, opt => opt.Ignore());
            CreateMap<Usuario, UsuarioDatosDTO>().ForMember(d => d.Id, opt => opt.MapFrom(o => o.Id));
            CreateMap<Usuario, UsuarioDatosVentaDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioRegistroDTO>().ReverseMap();
            CreateMap<UsuarioLoginDTO, UsuarioRespuestaLoginDTO>().ReverseMap();
        }
    }
}
