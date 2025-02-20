﻿using Core.Erp.Data.Facturacion.Base;
using Core.Erp.Data.General;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Facturacion
{
   public class fa_guia_remision_Data
    {
        tb_sis_Documento_Tipo_Talonario_Data data_talonario = new tb_sis_Documento_Tipo_Talonario_Data();
        fa_guia_remision_det_x_factura_Data data_guia_x_factura = new fa_guia_remision_det_x_factura_Data();
        fa_PuntoVta_Data data_puntovta = new fa_PuntoVta_Data();
        fa_factura_Data data_fact = new fa_factura_Data();
        fa_TerminoPago_Data data_tpago = new fa_TerminoPago_Data();

        public List<fa_guia_remision_Info> get_list(int IdEmpresa, DateTime fecha_inicio, DateTime Fecha_fin)
        {
            try
            {
                List<fa_guia_remision_Info> Lista;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = (from q in Context.vwfa_guia_remision
                             where q.IdEmpresa == IdEmpresa
                             && q.gi_fecha >= fecha_inicio
                             && q.gi_fecha <= Fecha_fin
                             select new fa_guia_remision_Info
                             {
                                IdEmpresa = q.IdEmpresa,
                                IdSucursal=q.IdSucursal,
                                IdBodega=q.IdBodega,
                                IdGuiaRemision=q.IdGuiaRemision,
                                CodGuiaRemision=q.CodGuiaRemision,
                                CodDocumentoTipo=q.CodDocumentoTipo,
                                Serie1=q.Serie1,
                                Serie2=q.Serie2,
                                NumGuia_Preimpresa=q.Serie1+"-"+q.Serie2+"-"+ q.NumGuia_Preimpresa,
                                NUAutorizacion=q.NUAutorizacion,
                                Fecha_Autorizacion=q.Fecha_Autorizacion,
                                IdCliente=q.IdCliente,
                                IdTransportista=q.IdTransportista,
                                gi_fecha=q.gi_fecha,
                                gi_plazo=q.gi_plazo,
                                gi_fech_venc=q.gi_fech_venc,
                                gi_Observacion=q.gi_Observacion,
                                gi_FechaInicioTraslado=q.gi_FechaInicioTraslado,
                                gi_FechaFinTraslado=q.gi_FechaFinTraslado,
                                placa=q.placa,
                                Direccion_Destino=q.Direccion_Destino,
                                Direccion_Origen=q.Direccion_Origen,
                                Estado=q.Estado,
                                pe_nombreCompleto=q.pe_nombreCompleto,
                                pe_cedulaRuc=q.pe_cedulaRuc,
                                IdCbteVta = q.IdCbteVta,
                                EstadoBool = q.Estado

                             }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public fa_guia_remision_Info get_info(int IdEmpresa, decimal IdGuiaRemision)
        {
            try
            {
                fa_guia_remision_Info info = new fa_guia_remision_Info();
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_guia_remision Entity = Context.fa_guia_remision.FirstOrDefault(q => q.IdGuiaRemision == IdGuiaRemision && q.IdEmpresa == IdEmpresa);
                    if (Entity == null) return null;
                    info = new fa_guia_remision_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal = Entity.IdSucursal,
                        IdBodega = Entity.IdBodega,
                        IdGuiaRemision = Entity.IdGuiaRemision,
                        CodGuiaRemision = Entity.CodGuiaRemision,
                        CodDocumentoTipo = Entity.CodDocumentoTipo,
                        Serie1 = Entity.Serie1,
                        Serie2 = Entity.Serie2,
                        NumGuia_Preimpresa = Entity.NumGuia_Preimpresa,
                        NUAutorizacion = Entity.NUAutorizacion,
                        Fecha_Autorizacion = Entity.Fecha_Autorizacion,
                        IdCliente = Entity.IdCliente,
                        IdTransportista = Entity.IdTransportista,
                        gi_fecha = Entity.gi_fecha,
                        gi_plazo = Entity.gi_plazo,
                        gi_fech_venc = Entity.gi_fech_venc,
                        gi_Observacion = Entity.gi_Observacion,
                        gi_FechaInicioTraslado = Entity.gi_FechaInicioTraslado,
                        gi_FechaFinTraslado = Entity.gi_FechaFinTraslado,
                        placa = Entity.placa,
                        Direccion_Destino = Entity.Direccion_Destino,
                        Direccion_Origen = Entity.Direccion_Origen,
                        Estado = Entity.Estado,
                        IdMotivoTraslado = Entity.IdMotivoTraslado,
                        IdCbteVta = Entity.IdCbteVta,
                        IdPuntoVta = Entity.IdPuntoVta,
                        IdContacto = (Entity.IdContacto == null ? 0 : Entity.IdContacto)

                        //GenerarFactura = (Entity.IdCbteVta != null || Entity.IdCbteVta != 0) ? true : false
                    };
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private decimal get_id(int IdEmpresa)
        {
            try
            {
                decimal ID = 1;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    var lst = from q in Context.fa_guia_remision
                              where q.IdEmpresa == IdEmpresa
                              select q;
                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdGuiaRemision) + 1;
                }
                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(fa_guia_remision_Info info)
        {
            fa_TerminoPago_Info termino_pago = new fa_TerminoPago_Info();
            fa_PuntoVta_Info punto_venta = new fa_PuntoVta_Info();
            termino_pago = data_tpago.get_info(info.vt_tipo_venta);
            punto_venta = data_puntovta.get_info(info.IdEmpresa, info.IdSucursal, info.IdPuntoVta_Fact);
            try
            {
                int secuencia = 1;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_guia_remision Entity = new fa_guia_remision
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdSucursal = info.IdSucursal,
                        IdPuntoVta = info.IdPuntoVta,                        
                        IdBodega = info.IdBodega,
                        IdGuiaRemision =info.IdGuiaRemision= get_id(info.IdEmpresa),
                        CodGuiaRemision = info.CodGuiaRemision,
                        CodDocumentoTipo = info.CodDocumentoTipo,
                        Serie1 = info.Serie1,
                        Serie2 = info.Serie2,
                        NumGuia_Preimpresa = info.NumGuia_Preimpresa,
                        NUAutorizacion = info.NUAutorizacion,
                        Fecha_Autorizacion = info.Fecha_Autorizacion,
                        IdCliente = info.IdCliente,
                        IdTransportista = info.IdTransportista,
                        gi_fecha = info.gi_fecha.Date,
                        gi_plazo = info.gi_plazo,
                        gi_fech_venc = info.gi_fech_venc,
                        gi_Observacion = info.gi_Observacion,
                        gi_FechaInicioTraslado = info.gi_FechaInicioTraslado.Date,
                        gi_FechaFinTraslado = info.gi_FechaFinTraslado.Date,
                        placa = info.placa,
                        Direccion_Destino = info.Direccion_Destino,
                        Direccion_Origen = info.Direccion_Origen,
                        IdMotivoTraslado=info.IdMotivoTraslado,
                        Estado = info.Estado= true,
                        IdUsuarioCreacion=info.IdUsuarioCreacion,
                        FechaCreacion=info.FechaCreacion = DateTime.Now,
                        IdCbteVta = info.IdCbteVta,
                        IdContacto = (info.IdContacto == 0 ? null : info.IdContacto)
                    };                    

                    foreach (var item in info.lst_detalle)
                    {
                        Context.fa_guia_remision_det.Add(new fa_guia_remision_det
                        {
                            IdEmpresa=info.IdEmpresa,
                            IdSucursal=info.IdSucursal,
                            IdBodega=info.IdBodega,
                            IdGuiaRemision=info.IdGuiaRemision,
                            Secuencia=secuencia,
                            IdProducto=item.IdProducto,
                            gi_cantidad=item.gi_cantidad,
                            gi_detallexItems =item.gi_detallexItems,
                            gi_precio = item.gi_precio,
                            gi_por_desc = item.gi_por_desc,
                            gi_descuentoUni = item.gi_descuentoUni,
                            gi_PrecioFinal = item.gi_PrecioFinal,
                            gi_Subtotal = item.gi_Subtotal,
                            IdCod_Impuesto = item.IdCod_Impuesto,
                            gi_por_iva = item.gi_por_iva,
                            gi_Iva = item.gi_Iva,
                            gi_Total = item.gi_Total,
                            IdCentroCosto = item.IdCentroCosto,
                            IdEmpresa_pf = item.IdEmpresa_pf,
                            IdSucursal_pf = item.IdSucursal_pf,
                            IdProforma = item.IdProforma,
                            Secuencia_pf = item.Secuencia_pf                            
                        });

                        if(item.IdCbteVta != null && item.IdCbteVta !=0)
                            Context.fa_guia_remision_det_x_factura.Add(new fa_guia_remision_det_x_factura
                            {
                                IdEmpresa_fact = info.IdEmpresa,
                                IdSucursal_fact = info.IdSucursal,
                                IdBodega_fact = info.IdBodega,
                                IdCbteVta_fact = Convert.ToDecimal(item.IdCbteVta),
                                IdGuiaRemision_guia = info.IdGuiaRemision,
                                Secuencia_fact = Convert.ToInt32(item.Secuencia_fact),
                                Secuencia_guia = secuencia,
                                IdBodega_guia = info.IdBodega,
                                IdSucursal_guia = info.IdSucursal,
                                IdEmpresa_guia = info.IdEmpresa
                            });

                        secuencia++;
                    }
                    
                    foreach (var item in info.lst_detalle_x_factura)
                    {
                        if (item.IdCbteVta != null && item.IdCbteVta != 0)
                            Context.fa_factura_x_fa_guia_remision.Add(new fa_factura_x_fa_guia_remision
                            {
                                fa_IdEmpresa = info.IdEmpresa,
                                fa_IdSucursal = info.IdSucursal,
                                fa_IdBodega = info.IdBodega,
                                fa_IdCbteVta = item.IdCbteVta,
                                gi_IdEmpresa = info.IdEmpresa,
                                gi_IdSucursal = info.IdSucursal,
                                gi_IdBodega = info.IdBodega,
                                gi_IdGuiaRemision = info.IdGuiaRemision
                            });
                    }

                    #region Talonario
                    fa_PuntoVta_Info info_puntovta = new fa_PuntoVta_Info();
                    tb_sis_Documento_Tipo_Talonario_Info info_talonario = new tb_sis_Documento_Tipo_Talonario_Info();
                    tb_sis_Documento_Tipo_Talonario_Info ultimo_talonario = new tb_sis_Documento_Tipo_Talonario_Info();
                    tb_sis_Documento_Tipo_Talonario_Data data_talonario = new tb_sis_Documento_Tipo_Talonario_Data();
                    info_puntovta = data_puntovta.get_info(info.IdEmpresa, info.IdSucursal, info.IdPuntoVta);

                    if (info_puntovta != null)
                    {
                        if (info_puntovta.EsElectronico == true)
                        {
                            ultimo_talonario = data_talonario.GetUltimoNoUsado(info.IdEmpresa, info_puntovta.codDocumentoTipo, info_puntovta.Su_CodigoEstablecimiento, info_puntovta.cod_PuntoVta, info_puntovta.EsElectronico, true);

                            if (ultimo_talonario != null)
                            {
                                Entity.Serie1 = info.Serie1 = ultimo_talonario.Establecimiento;
                                Entity.Serie2 = info.Serie2 = ultimo_talonario.PuntoEmision;
                                Entity.NumGuia_Preimpresa = info.NumGuia_Preimpresa = ultimo_talonario.NumDocumento;
                                Entity.NUAutorizacion = null;
                                Entity.Fecha_Autorizacion = null;
                            }
                        }
                        else
                        {
                            info_talonario.IdEmpresa = info.IdEmpresa;
                            info_talonario.CodDocumentoTipo = info.CodDocumentoTipo;
                            info_talonario.Establecimiento = info.Serie1;
                            info_talonario.PuntoEmision = info.Serie2;
                            info_talonario.NumDocumento = info.NumGuia_Preimpresa;
                            info_talonario.IdSucursal = info.IdSucursal;
                            info_talonario.Usado = true;

                            Entity.NUAutorizacion = info_talonario.NumAutorizacion;
                            Entity.Fecha_Autorizacion = DateTime.Now.Date;

                            data_talonario.modificar_estado_usadoDB(info_talonario);
                        }
                    }

                    #endregion

                    #region Factura
                    if (info.GenerarFactura == true && (info.IdCbteVta == null || info.IdCbteVta == 0))
                    {
                        fa_factura_Info info_fact = new fa_factura_Info();
                        info_fact.lst_det = new List<fa_factura_det_Info>();
                        info_fact.lst_cuota = new List<fa_cuotas_x_doc_Info>();
                        List<fa_guia_remision_det_x_factura_Info> info_guia_x_factura = new List<fa_guia_remision_det_x_factura_Info>();
                        int secuencia_fact = 1;

                        if (termino_pago != null && termino_pago.IdTerminoPago != "")
                        {
                            info.vt_fech_venc = info.gi_fecha.AddDays(termino_pago.Dias_Vct);
                        }

                        if (punto_venta != null && punto_venta.IdPuntoVta != 0)
                        {
                            info.IdCaja = punto_venta.IdCaja;
                        }

                        info_fact.IdEmpresa = info_fact.IdEmpresa = info.IdEmpresa;
                        info_fact.IdSucursal = info_fact.IdSucursal = info.IdSucursal;
                        info_fact.IdBodega = info_fact.IdBodega = info.IdBodega;
                        info_fact.vt_tipoDoc = cl_enumeradores.eTipoDocumento.FACT.ToString();
                        info_fact.vt_serie1 = info.vt_serie1;
                        info_fact.vt_serie2 = info.vt_serie2;
                        info_fact.vt_NumFactura = info.vt_NumFactura;
                        info_fact.Fecha_Autorizacion = DateTime.Now;
                        info_fact.vt_autorizacion = null;
                        info_fact.vt_fecha = info.gi_fecha.Date;
                        info_fact.vt_fech_venc = info.vt_fech_venc; /*depende de la termino de pago*/
                        info_fact.IdCliente = info.IdCliente;
                        info_fact.IdContacto = info.IdContacto;
                        info_fact.IdVendedor = info.IdVendedor;
                        info_fact.vt_plazo = termino_pago.Dias_Vct; /**depende de la termino de pago*/
                        info_fact.vt_Observacion = string.IsNullOrEmpty(info.ObservacionFactura) ? "" : info.ObservacionFactura;
                        info_fact.IdCatalogo_FormaPago = info.IdCatalogo_FormaPago;
                        info_fact.vt_tipo_venta = info.vt_tipo_venta;
                        info_fact.IdCaja = info.IdCaja; /*depende del punto de venta*/
                        info_fact.IdPuntoVta = info.IdPuntoVta_Fact;
                        info_fact.fecha_primera_cuota = null;
                        info_fact.Fecha_Transaccion = DateTime.Now;
                        info_fact.Estado = info.Estado == true ? "A" : "I";
                        info_fact.esta_impresa = null;
                        info_fact.valor_abono = null;
                        info_fact.IdUsuario = info.IdUsuarioCreacion;
                        info_fact.IdNivel = 1;
                        info_fact.CodCbteVta = info.CodGuiaRemision;

                        var lst_detalle_factura = info.lst_detalle.Where(q=>q.SeFactura==true);
                        foreach (var item in info.lst_detalle)
                        {
                            fa_factura_det_Info info_fact_detalle = new fa_factura_det_Info
                            {
                                IdEmpresa = info_fact.IdEmpresa,
                                IdSucursal = info_fact.IdSucursal,
                                IdBodega = info_fact.IdBodega,
                                Secuencia = secuencia_fact++,

                                IdProducto = item.IdProducto,
                                vt_cantidad = item.gi_cantidad,
                                vt_Precio = item.gi_precio,
                                vt_PorDescUnitario = item.gi_por_desc,
                                vt_DescUnitario = item.gi_descuentoUni,
                                vt_PrecioFinal = item.gi_PrecioFinal,
                                vt_Subtotal = item.gi_Subtotal,
                                vt_por_iva = item.gi_por_iva,
                                IdCod_Impuesto_Iva = item.IdCod_Impuesto,
                                vt_iva = item.gi_Iva,
                                vt_total = item.gi_Total,
                                vt_detallexItems = item.gi_detallexItems,
                                IdEmpresa_pf = item.IdEmpresa_pf,
                                IdSucursal_pf = item.IdSucursal_pf,
                                IdProforma = item.IdProforma,
                                Secuencia_pf = item.Secuencia_pf,

                                IdCentroCosto = item.IdCentroCosto,
                                IdPunto_Cargo = item.IdPunto_cargo,
                                IdPunto_cargo_grupo = item.IdPunto_cargo_grupo,
                            };

                            info_fact.lst_det.Add(info_fact_detalle);
                        }

                        var SubtotalConDscto = (decimal)Math.Round(info.lst_detalle.Sum(q => q.gi_Subtotal), 2, MidpointRounding.AwayFromZero);
                        var SubtotalIVASinDscto = (decimal)Math.Round(info_fact.lst_det.Where(q => q.vt_por_iva != 0).Sum(q => q.vt_cantidad * q.vt_Precio), 2, MidpointRounding.AwayFromZero);
                        var SubtotalSinIVASinDscto = (decimal)Math.Round(info_fact.lst_det.Where(q => q.vt_por_iva == 0).Sum(q => q.vt_cantidad * q.vt_Precio), 2, MidpointRounding.AwayFromZero);
                        var Descuento = (decimal)Math.Round(info_fact.lst_det.Sum(q => q.vt_DescUnitario * q.vt_cantidad), 2, MidpointRounding.AwayFromZero);
                        var SubtotalIVAConDscto = (decimal)Math.Round(info_fact.lst_det.Where(q => q.vt_por_iva != 0).Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero);
                        var SubtotalSinIVAConDscto = (decimal)Math.Round(info_fact.lst_det.Where(q => q.vt_por_iva == 0).Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero);
                        var ValorIVA = (decimal)Math.Round(info_fact.lst_det.Sum(q => q.vt_iva), 2, MidpointRounding.AwayFromZero);
                        var SubtotalSinDscto = SubtotalIVASinDscto + SubtotalSinIVASinDscto;
                        var Total = SubtotalConDscto + ValorIVA;

                        info_fact.info_resumen = new fa_factura_resumen_Info
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdSucursal = info.IdSucursal,
                            IdBodega = info.IdBodega,
                            //  IdCbteVta =  IdCbteVta,

                            SubtotalConDscto = SubtotalConDscto,
                            SubtotalIVAConDscto = SubtotalIVAConDscto,
                            SubtotalIVASinDscto = SubtotalIVASinDscto,
                            SubtotalSinDscto = SubtotalSinDscto,
                            SubtotalSinIVAConDscto = SubtotalSinIVAConDscto,
                            SubtotalSinIVASinDscto = SubtotalSinIVASinDscto,

                            Total = Total,
                            Descuento = Descuento,
                            ValorEfectivo = 0,
                            ValorIVA = ValorIVA,
                            Cambio = Total,
                        };
                        data_fact.guardarDB(info_fact);
                        Entity.IdCbteVta = info_fact.IdCbteVta;

                        #region Relacion Guia
                        Context.fa_factura_x_fa_guia_remision.Add(new fa_factura_x_fa_guia_remision
                        {
                            fa_IdEmpresa = info.IdEmpresa,
                            fa_IdSucursal = info.IdSucursal,
                            fa_IdBodega = info.IdBodega,
                            fa_IdCbteVta = info_fact.IdCbteVta,
                            gi_IdEmpresa = info.IdEmpresa,
                            gi_IdSucursal = info.IdSucursal,
                            gi_IdBodega = info.IdBodega,
                            gi_IdGuiaRemision = info.IdGuiaRemision
                        });
                        secuencia = 1;
                        foreach (var item in info.lst_detalle)
                        {
                            Context.fa_guia_remision_det_x_factura.Add(new fa_guia_remision_det_x_factura
                            {
                                IdEmpresa_fact = info.IdEmpresa,
                                IdSucursal_fact = info.IdSucursal,
                                IdBodega_fact = info.IdBodega,
                                IdCbteVta_fact = info_fact.IdCbteVta,
                                Secuencia_fact = secuencia,

                                IdEmpresa_guia = info.IdEmpresa,
                                IdSucursal_guia = info.IdSucursal,
                                IdBodega_guia = info.IdBodega,
                                IdGuiaRemision_guia = info.IdGuiaRemision,
                                Secuencia_guia = secuencia++,
                            });
                        }
                        #endregion
                    }
                    #endregion

                    Context.fa_guia_remision.Add(Entity);
                    Context.SaveChanges();                    
                }
                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.InnerException.Message, Clase = "fa_guia_remision_Data", Metodo = "guardarDB", IdUsuario = info.IdUsuarioCreacion });
                return false;
            }
        }
        public bool modificarDB(fa_guia_remision_Info info)
        {
            try
            {
                fa_TerminoPago_Info termino_pago = new fa_TerminoPago_Info();
                fa_PuntoVta_Info punto_venta = new fa_PuntoVta_Info();
                termino_pago = data_tpago.get_info(info.vt_tipo_venta);
                punto_venta = data_puntovta.get_info(info.IdEmpresa, info.IdSucursal, info.IdPuntoVta_Fact);
                int secuencia = 1;

                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_guia_remision Entity = Context.fa_guia_remision.FirstOrDefault(q => q.IdGuiaRemision == info.IdGuiaRemision);
                    if (Entity == null) return false;
                       Entity.CodGuiaRemision = info.CodGuiaRemision;
                        Entity.CodDocumentoTipo = info.CodDocumentoTipo;
                        Entity.NUAutorizacion = info.NUAutorizacion;
                        Entity.Fecha_Autorizacion = info.Fecha_Autorizacion;
                        Entity.IdCliente = info.IdCliente;
                        Entity.IdTransportista = info.IdTransportista;
                        Entity.gi_fecha = info.gi_fecha.Date;
                        Entity.gi_plazo = info.gi_plazo;
                        Entity.gi_fech_venc = info.gi_fech_venc;
                        Entity.gi_FechaInicioTraslado = info.gi_FechaInicioTraslado.Date;
                        Entity.gi_FechaFinTraslado = info.gi_FechaFinTraslado.Date;
                        Entity.placa = info.placa;
                        Entity.Direccion_Destino = info.Direccion_Destino;
                        Entity.Direccion_Origen = info.Direccion_Origen;
                        Entity.IdMotivoTraslado = info.IdMotivoTraslado;
                        Entity.FechaModificacion = info.FechaModificacion = DateTime.Now;
                        Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
                        Entity.gi_Observacion = info.gi_Observacion;
                        Entity.IdCbteVta = info.IdCbteVta;
                        Entity.IdContacto = (info.IdContacto == 0 ? null : info.IdContacto);

                    foreach (var item in info.lst_detalle)
                    {
                        Context.fa_guia_remision_det.Add(new fa_guia_remision_det
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdSucursal = info.IdSucursal,
                            IdBodega = info.IdBodega,
                            IdGuiaRemision = info.IdGuiaRemision,
                            Secuencia = secuencia,
                            IdProducto = item.IdProducto,
                            gi_cantidad = item.gi_cantidad,
                            gi_detallexItems = item.gi_detallexItems,
                            gi_precio = item.gi_precio,
                            gi_por_desc = item.gi_por_desc,
                            gi_descuentoUni = item.gi_descuentoUni,
                            gi_PrecioFinal = item.gi_PrecioFinal,
                            gi_Subtotal = item.gi_Subtotal,
                            IdCod_Impuesto = item.IdCod_Impuesto,
                            gi_por_iva = item.gi_por_iva,
                            gi_Iva = item.gi_Iva,
                            gi_Total = item.gi_Total,
                            IdCentroCosto = item.IdCentroCosto,
                            IdEmpresa_pf = item.IdEmpresa_pf,
                            IdSucursal_pf = item.IdSucursal_pf,
                            IdProforma = item.IdProforma,
                            Secuencia_pf = item.Secuencia_pf
                        });

                        if (item.IdCbteVta != null && item.IdCbteVta != 0)
                            Context.fa_guia_remision_det_x_factura.Add(new fa_guia_remision_det_x_factura
                            {
                                IdEmpresa_fact = info.IdEmpresa,
                                IdSucursal_fact = info.IdSucursal,
                                IdBodega_fact = info.IdBodega,
                                IdCbteVta_fact = Convert.ToDecimal(item.IdCbteVta),
                                IdGuiaRemision_guia = info.IdGuiaRemision,
                                Secuencia_fact = Convert.ToInt32(item.Secuencia_fact),
                                Secuencia_guia = secuencia,
                                IdBodega_guia = info.IdBodega,
                                IdSucursal_guia=info.IdSucursal,
                                IdEmpresa_guia=info.IdEmpresa
                        
                            });
                        secuencia++;
                    }
                    foreach (var item in info.lst_detalle_x_factura)
                    {
                        if (item.IdCbteVta != 0)
                            Context.fa_factura_x_fa_guia_remision.Add(new fa_factura_x_fa_guia_remision
                            {
                                fa_IdEmpresa = info.IdEmpresa,
                                fa_IdSucursal = info.IdSucursal,
                                fa_IdBodega = info.IdBodega,
                                fa_IdCbteVta = item.IdCbteVta,
                                gi_IdEmpresa = info.IdEmpresa,
                                gi_IdSucursal = info.IdSucursal,
                                gi_IdBodega = info.IdBodega,
                                gi_IdGuiaRemision = info.IdGuiaRemision
                            });
                    }

                    #region Factura
                    if (info.GenerarFactura == true && (info.IdCbteVta == null || info.IdCbteVta == 0))
                    {
                        fa_factura_Info info_fact = new fa_factura_Info();
                        info_fact.lst_det = new List<fa_factura_det_Info>();
                        info_fact.lst_cuota = new List<fa_cuotas_x_doc_Info>();
                        int secuencia_fact = 1;

                        if (termino_pago != null && termino_pago.IdTerminoPago != "")
                        {
                            info.vt_fech_venc = info.gi_fecha.AddDays(termino_pago.Dias_Vct);
                        }

                        if (punto_venta != null && punto_venta.IdPuntoVta != 0)
                        {
                            info.IdCaja = punto_venta.IdCaja;
                        }

                        info_fact.IdEmpresa = info_fact.IdEmpresa = info.IdEmpresa;
                        info_fact.IdSucursal = info_fact.IdSucursal = info.IdSucursal;
                        info_fact.IdBodega = info_fact.IdBodega = info.IdBodega;
                        info_fact.vt_tipoDoc = cl_enumeradores.eTipoDocumento.FACT.ToString();
                        info_fact.vt_serie1 = info.vt_serie1;
                        info_fact.vt_serie2 = info.vt_serie2;
                        info_fact.vt_NumFactura = info.vt_NumFactura;
                        info_fact.Fecha_Autorizacion = DateTime.Now;
                        info_fact.vt_autorizacion = null;
                        info_fact.vt_fecha = info.gi_fecha.Date;
                        info_fact.vt_fech_venc = info.vt_fech_venc; /*depende de la termino de pago*/
                        info_fact.IdCliente = info.IdCliente;
                        info_fact.IdContacto = info.IdContacto;
                        info_fact.IdVendedor = info.IdVendedor;
                        info_fact.vt_plazo = termino_pago.Dias_Vct; /**depende de la termino de pago*/
                        info_fact.vt_Observacion = string.IsNullOrEmpty(info.ObservacionFactura) ? "" : info.ObservacionFactura;
                        info_fact.IdCatalogo_FormaPago = info.IdCatalogo_FormaPago;
                        info_fact.vt_tipo_venta = info.vt_tipo_venta;
                        info_fact.IdCaja = info.IdCaja; /*depende del punto de venta*/
                        info_fact.IdPuntoVta = info.IdPuntoVta_Fact;
                        info_fact.fecha_primera_cuota = null;
                        info_fact.Fecha_Transaccion = DateTime.Now;
                        info_fact.Estado = info.Estado == true ? "A" : "I";
                        info_fact.esta_impresa = null;
                        info_fact.valor_abono = null;
                        info_fact.IdUsuario = info.IdUsuarioCreacion;
                        info_fact.IdNivel = 1;
                        info_fact.CodCbteVta = info.CodGuiaRemision;

                        foreach (var item in info.lst_detalle)
                        {
                            fa_factura_det_Info info_fact_detalle = new fa_factura_det_Info
                            {
                                IdEmpresa = info_fact.IdEmpresa,
                                IdSucursal = info_fact.IdSucursal,
                                IdBodega = info_fact.IdBodega,
                                Secuencia = secuencia_fact++,

                                IdProducto = item.IdProducto,
                                vt_cantidad = item.gi_cantidad,
                                vt_Precio = item.gi_precio,
                                vt_PorDescUnitario = item.gi_por_desc,
                                vt_DescUnitario = item.gi_descuentoUni,
                                vt_PrecioFinal = item.gi_PrecioFinal,
                                vt_Subtotal = item.gi_Subtotal,
                                vt_por_iva = item.gi_por_iva,
                                IdCod_Impuesto_Iva = item.IdCod_Impuesto,
                                vt_iva = item.gi_Iva,
                                vt_total = item.gi_Total,
                                vt_detallexItems = item.gi_detallexItems,

                                IdEmpresa_pf = item.IdEmpresa_pf,
                                IdSucursal_pf = item.IdSucursal_pf,
                                IdProforma = item.IdProforma,
                                Secuencia_pf = item.Secuencia_pf,

                                IdCentroCosto = item.IdCentroCosto,
                                IdPunto_Cargo = item.IdPunto_cargo,
                                IdPunto_cargo_grupo = item.IdPunto_cargo_grupo,
                            };
                            info_fact.lst_det.Add(info_fact_detalle);
                        }

                        var SubtotalConDscto = (decimal)Math.Round(info.lst_detalle.Sum(q => q.gi_Subtotal), 2, MidpointRounding.AwayFromZero);
                        var SubtotalIVASinDscto = (decimal)Math.Round(info_fact.lst_det.Where(q => q.vt_por_iva != 0).Sum(q => q.vt_cantidad * q.vt_Precio), 2, MidpointRounding.AwayFromZero);
                        var SubtotalSinIVASinDscto = (decimal)Math.Round(info_fact.lst_det.Where(q => q.vt_por_iva == 0).Sum(q => q.vt_cantidad * q.vt_Precio), 2, MidpointRounding.AwayFromZero);
                        var Descuento = (decimal)Math.Round(info_fact.lst_det.Sum(q => q.vt_DescUnitario * q.vt_cantidad), 2, MidpointRounding.AwayFromZero);
                        var SubtotalIVAConDscto = (decimal)Math.Round(info_fact.lst_det.Where(q => q.vt_por_iva != 0).Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero);
                        var SubtotalSinIVAConDscto = (decimal)Math.Round(info_fact.lst_det.Where(q => q.vt_por_iva == 0).Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero);
                        var ValorIVA = (decimal)Math.Round(info_fact.lst_det.Sum(q => q.vt_iva), 2, MidpointRounding.AwayFromZero);
                        var SubtotalSinDscto = SubtotalIVASinDscto + SubtotalSinIVASinDscto;
                        var Total = SubtotalConDscto + ValorIVA;

                        info_fact.info_resumen = new fa_factura_resumen_Info
                        {

                            IdEmpresa = info_fact.IdEmpresa,
                            IdSucursal = info_fact.IdSucursal,
                            IdBodega = info_fact.IdBodega,
                            // IdCbteVta = info_fact.IdCbteVta,

                            SubtotalConDscto = SubtotalConDscto,
                            SubtotalIVAConDscto = SubtotalIVAConDscto,
                            SubtotalIVASinDscto = SubtotalIVASinDscto,
                            SubtotalSinDscto = SubtotalSinDscto,
                            SubtotalSinIVAConDscto = SubtotalSinIVAConDscto,
                            SubtotalSinIVASinDscto = SubtotalSinIVASinDscto,

                            Total = Total,
                            Descuento = Descuento,
                            ValorEfectivo = 0,
                            ValorIVA = ValorIVA,
                            Cambio = Total,
                        };
                        data_fact.guardarDB(info_fact);
                        Entity.IdCbteVta = info_fact.IdCbteVta;

                        #region Relacion Guia
                        Context.fa_factura_x_fa_guia_remision.Add(new fa_factura_x_fa_guia_remision
                        {
                            fa_IdEmpresa = info.IdEmpresa,
                            fa_IdSucursal = info.IdSucursal,
                            fa_IdBodega = info.IdBodega,
                            fa_IdCbteVta = info_fact.IdCbteVta,
                            gi_IdEmpresa = info.IdEmpresa,
                            gi_IdSucursal = info.IdSucursal,
                            gi_IdBodega = info.IdBodega,
                            gi_IdGuiaRemision = info.IdGuiaRemision
                        });
                        secuencia = 1;
                        foreach (var item in info.lst_detalle)
                        {
                            Context.fa_guia_remision_det_x_factura.Add(new fa_guia_remision_det_x_factura
                            {
                                IdEmpresa_fact = info.IdEmpresa,
                                IdSucursal_fact = info.IdSucursal,
                                IdBodega_fact = info.IdBodega,
                                IdCbteVta_fact = info_fact.IdCbteVta,
                                Secuencia_fact = secuencia,

                                IdEmpresa_guia = info.IdEmpresa,
                                IdSucursal_guia = info.IdSucursal,
                                IdBodega_guia = info.IdBodega,
                                IdGuiaRemision_guia = info.IdGuiaRemision,
                                Secuencia_guia = secuencia++,
                            });
                        }
                        #endregion
                    }
                    #endregion

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_guia_remision_Data", Metodo = "modificarDB", IdUsuario = info.IdUsuarioCreacion });
                return false;
            }
        }
        public bool anularDB(fa_guia_remision_Info info)
        {
            try
            {
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_guia_remision Entity = Context.fa_guia_remision.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdGuiaRemision == info.IdGuiaRemision);
                    List<fa_guia_remision_det> EntityDet = Context.fa_guia_remision_det.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdGuiaRemision == info.IdGuiaRemision).ToList();

                    if (Entity == null) return false;
                    Entity.Estado = info.Estado = false;
                    Entity.IdUsuarioAnulacion = info.IdUsuarioAnulacion;
                    Entity.FechaAnulacion = DateTime.Now;
                    Entity.MotivoAnulacion = info.MotiAnula;

                    foreach (var item in EntityDet)
                    {
                        item.IdEmpresa_pf = null;
                        item.IdSucursal_pf = null;
                        item.Secuencia_pf = null;
                        item.IdProforma = null;
                    }

                    var lst_det = Context.fa_guia_remision_det_x_factura.Where(q => q.IdEmpresa_guia == info.IdEmpresa && q.IdGuiaRemision_guia == info.IdGuiaRemision).ToList();
                    Context.fa_guia_remision_det_x_factura.RemoveRange(lst_det);

                    var lst_fac = Context.fa_factura_x_fa_guia_remision.Where(q => q.gi_IdEmpresa == info.IdEmpresa && q.gi_IdGuiaRemision == info.IdGuiaRemision).ToList();
                    Context.fa_factura_x_fa_guia_remision.RemoveRange(lst_fac);

                    Context.SaveChanges();

                    
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool si_existe(int IdEmpresa, string Establecimiento, string Puntoemision, string Numero)
        {
            try
            {
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    var lst = from q in Context.fa_guia_remision
                              where q.IdEmpresa == IdEmpresa
                              && q.Serie1==Establecimiento
                              && q.Serie2==Puntoemision
                              && q.NumGuia_Preimpresa==null
                              select q;
                    if (lst.Count() > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarEstadoAutorizacion(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdGuiaRemision)
        {
            try
            {
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    var Entity = Context.fa_guia_remision.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdGuiaRemision == IdGuiaRemision).FirstOrDefault();
                    if (Entity != null)
                    {
                        Entity.aprobada_enviar_sri = true;
                        Context.SaveChanges();

                        var Factura = Context.fa_factura.Where(q => q.IdEmpresa == Entity.IdEmpresa && q.IdSucursal == Entity.IdSucursal && q.IdBodega == Entity.IdBodega && q.IdCbteVta == Entity.IdCbteVta).FirstOrDefault();
                        if (Factura != null)
                        {
                            Factura.aprobada_enviar_sri = true;
                            Context.SaveChanges();
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<fa_guia_remision_Info> get_list_x_contacto(int IdEmpresa, decimal IdCliente, decimal IdContacto)
        {
            try
            {
                List<fa_guia_remision_Info> Lista;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = (from q in Context.fa_guia_remision
                             where q.IdEmpresa == IdEmpresa
                             && q.IdCliente == IdCliente
                             && q.IdContacto == IdContacto
                             select new fa_guia_remision_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega = q.IdBodega,
                                 IdGuiaRemision = q.IdGuiaRemision,
                                 CodGuiaRemision = q.CodGuiaRemision,
                                 CodDocumentoTipo = q.CodDocumentoTipo
                             }).ToList();
                }
                return Lista;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_factura_Data", Metodo = "get_list_fac_sin_guia", IdUsuario = "consulta" });
                return new List<fa_guia_remision_Info>();
            }
        }

    }
}
