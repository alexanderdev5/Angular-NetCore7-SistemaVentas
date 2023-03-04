using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.Utility
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            #region Producto
            CreateMap<Producto, ProductoDTO>().
              ForMember(destino =>
              destino.DescripcionCategoria,
              opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Nombre)
              )
              .ForMember(destino =>
              destino.Precio,
              opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-PE")))
              )
              .ForMember(destino =>
              destino.EsActivo,
              opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0)
              );

            CreateMap<ProductoDTO, Producto>().
             ForMember(destino =>
             destino.IdCategoriaNavigation,
             opt => opt.Ignore()
             )
             .ForMember(destino =>
             destino.Precio,
             opt => opt.MapFrom(origen => Convert.ToString(origen.Precio, new CultureInfo("es-PE")))
             )
             .ForMember(destino =>
             destino.EsActivo,
             opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false)             );

            #endregion Producto


            #region Venta
            CreateMap<Venta, VentaDTO>().
             ForMember(destino =>
             destino.TotalTexto,
             opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-PE")))
             ).
             ForMember(destino =>
             destino.FechaRegistro,
             opt => opt.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy"))
             );
            CreateMap<VentaDTO, Venta>()
             .ForMember(destino =>
             destino.Total,
             opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-PE")))
             );

            #endregion Venta


            #region  DetalleVenta   

            CreateMap<DetalleVenta, DetalleVentaDTO>()
             .ForMember(destino => destino.DescripcionProductos,
             opt => opt.MapFrom(origen => origen.IdProductoNavigation.Nombre)
             )
             .ForMember(destino => destino.PrecioTexto,
             opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-PE")))
             )
             .ForMember(destino =>
             destino.TotalTexto,
             opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-PE")))
             );

            CreateMap<DetalleVentaDTO, DetalleVenta>()
              .ForMember(destino => destino.Precio,
             opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-PE")))
             ).
             ForMember(destino => destino.Total,
             opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-PE")))
             );
            #endregion DetalleVenta






        }



    }
}
