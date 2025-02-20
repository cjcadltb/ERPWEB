﻿using Core.Erp.Info.Reportes.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
using Core.Erp.Data.General;

namespace Core.Erp.Data.Reportes.CuentasPorPagar
{
    public class CXP_013_Data
    {
        public List<CXP_013_Info> get_list(int IdEmpresa, decimal IdRetencion)
        {
            try
            {
                List<CXP_013_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWCXP_013
                             where q.IdEmpresa == IdEmpresa
                             && q.IdRetencion == IdRetencion
                             select new CXP_013_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdRetencion = q.IdRetencion,
                                 Idsecuencia = q.Idsecuencia,
                                 re_TipoRet = q.re_TipoRet,
                                 co_factura = q.co_factura,
                                 NumRetencion = q.NumRetencion,
                                 TipoComprobante = q.TipoComprobante,
                                 FechaDeEmision = q.FechaDeEmision,
                                 EjercicioFiscal = q.EjercicioFiscal,
                                 re_baseRetencion = q.re_baseRetencion,
                                 re_Porcen_retencion = q.re_Porcen_retencion,
                                 re_valor_retencion = q.re_valor_retencion,
                                 NombreProveedor = q.NombreProveedor,
                                 pr_direccion = q.pr_direccion,
                                 pe_cedulaRuc = q.pe_cedulaRuc,
                                 pr_correo = q.pr_correo,
                                 pr_telefonos = q.pr_telefonos,
                                 NAutorizacion = q.NAutorizacion,
                                 Fecha_Autorizacion = q.Fecha_Autorizacion,
                                 Su_Descripcion = q.Su_Descripcion,
                                 co_FechaFactura = q.co_FechaFactura
                             }).ToList();
                }

                if (Lista.Count > 0)
                {
                    var Detalle = Lista[0];
                    if (!string.IsNullOrEmpty(Detalle.NumRetencion) && string.IsNullOrEmpty(Detalle.NAutorizacion))
                    {
                        tb_empresa_Data odataEmpresa = new tb_empresa_Data();
                        tb_sis_Documento_Tipo_Talonario_Data odataTalonario = new tb_sis_Documento_Tipo_Talonario_Data();
                        string[] Array = Detalle.NumRetencion.Split('-');
                        if (Array.Count() == 3)
                        {
                            string ClaveAcceso = odataTalonario.GeneraClaveAcceso(Detalle.FechaDeEmision, "07", odataEmpresa.get_info(IdEmpresa).em_ruc, Array[0] + Array[1], Array[2]);
                            Lista.ForEach(q => q.NAutorizacion = ClaveAcceso);
                        }
                    }
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
