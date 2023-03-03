using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Agregando las referencias
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;


namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbsistemaventasContext _dbcontext;

        public VentaRepository(DbsistemaventasContext dbcontext):base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
           
              Venta ventaGenerada = new Venta();
                 
              using (var transaction = _dbcontext.Database.BeginTransaction()){


                    try
                    {
                        foreach(DetalleVenta dv in modelo.DetalleVenta)
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

                        int CantidadDigitos = 4;
                        string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));
                        string numeroVenta = ceros + correlativo.UltimoNumero.ToString();

                        //00001
                        numeroVenta = numeroVenta.Substring(numeroVenta.Length - CantidadDigitos, CantidadDigitos);

                        modelo.NumeroDocumento = numeroVenta;
                        await _dbcontext.Venta.AddAsync(modelo);
                        await _dbcontext.SaveChangesAsync();

                        ventaGenerada = modelo;

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
