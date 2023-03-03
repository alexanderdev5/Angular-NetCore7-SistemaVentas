using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;


namespace SistemaVenta.DAL.Repositorios
{
    public interface IVentaRepository: IGenericRepository<Venta>
    {
        Task<Venta> Registrar(Venta modelo);


    }
}
