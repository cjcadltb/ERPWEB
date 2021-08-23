﻿using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Inventario;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Caja;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    [SessionTimeout]
    public class EgresoInventarioController : Controller
    {
        #region Variables
        in_Ing_Egr_Inven_Bus bus_ing_inv = new in_Ing_Egr_Inven_Bus();
        in_Ing_Egr_Inven_det_Bus bus_det_ing_inv = new in_Ing_Egr_Inven_det_Bus();
        in_Ing_Egr_Inven_det_List List_in_Ing_Egr_Inven_det = new in_Ing_Egr_Inven_det_List();
        in_parametro_Bus bus_in_param = new in_parametro_Bus();
        in_Producto_Bus bus_producto = new in_Producto_Bus();
        in_Motivo_Inven_Bus bus_motivo = new in_Motivo_Inven_Bus();
        string mensaje = string.Empty;
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        in_UnidadMedida_Equiv_conversion_Bus bus_UnidadMedidaEquivalencia = new in_UnidadMedida_Equiv_conversion_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        ct_CentroCosto_Bus bus_cc = new ct_CentroCosto_Bus();
        in_producto_x_tb_bodega_Costo_Historico_Bus bus_prod_x_bod = new in_producto_x_tb_bodega_Costo_Historico_Bus();
        in_producto_x_tb_bodega_Bus bus_producto_x_bodega = new in_producto_x_tb_bodega_Bus();
        in_Egreso_Inven_List Lista_Egreso = new in_Egreso_Inven_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        #endregion

        #region Metodos ComboBox bajo demanda
        public ActionResult CmbProducto_EgresoInventario()
      {
            decimal model = new decimal();
            return PartialView("_CmbProducto_EgresoInventario", model);
        }
        public List<in_Producto_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);
            int IdSucursal = string.IsNullOrEmpty(SessionFixed.IdSucursalInv) ? 0 : Convert.ToInt32(SessionFixed.IdSucursalInv);
            int IdBodega = string.IsNullOrEmpty(SessionFixed.IdBodegaInv) ? 0 : Convert.ToInt32(SessionFixed.IdBodegaInv);

            return bus_producto.get_list_bajo_demanda(args, IdEmpresa,cl_enumeradores.eTipoBusquedaProducto.PORBODEGA,cl_enumeradores.eModulo.INV,IdSucursal,IdBodega);
        }
        public in_Producto_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_producto.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Metodos ComboBox bajo demanda persona
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        public ActionResult CmbPersona_EgresoInv()
        {
            //SessionFixed.TipoPersona = Request.Params["IdTipoPersona"] != null ? Request.Params["IdTipoPersona"].ToString() : SessionFixed.TipoPersona;
            decimal model = new decimal();
            return PartialView("_CmbPersona_EgresoInv", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda_persona(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), "PERSONA");
        }
        public tb_persona_Info get_info_bajo_demanda_persona(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), "PERSONA");
        }
        #endregion

        #region Metodos ComboBox bajo demanda centro de costo

        public ActionResult CmbCentroCosto_Inv_Eg()
        {
            string model = string.Empty;
            return PartialView("_CmbCentroCosto_Inv_Eg", model);
        }
        public List<ct_CentroCosto_Info> get_list_bajo_demandaEg(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            List<ct_CentroCosto_Info> Lista = bus_cc.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
            return Lista;
        }
        public ct_CentroCosto_Info get_info_bajo_demandaEg(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_cc.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Metodos ComboBox bajo demanda motivo
        public ActionResult CmbMotivoInven()
        {
            decimal model = new decimal();
            return PartialView("_CmbMotivoInven", model);
        }
        public List<in_Motivo_Inven_Info> get_list_bajo_demanda_motivo(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_motivo.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoIngEgr.EGR.ToString());
        }
        public in_Motivo_Inven_Info get_info_bajo_demanda_motivo(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_motivo.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region vistas
        [ValidateInput(false)]
        public ActionResult GridViewPartial_egr_inv_det()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_egr_inv_det", model);
        }
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "EgresoInventario", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date
            };
            CargarCombosConsulta(model.IdEmpresa);
            var lst = bus_ing_inv.get_list(model.IdEmpresa, "-", model.IdSucursal, true, model.fecha_ini, model.fecha_fin);
            Lista_Egreso.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        private void CargarCombosConsulta(int IdEmpresa)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
            ViewBag.lst_sucursal = lst_sucursal;
        }

        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "EgresoInventario", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion
            CargarCombosConsulta(model.IdEmpresa);
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            var lst = bus_ing_inv.get_list(model.IdEmpresa, "-", model.IdSucursal, true, model.fecha_ini, model.fecha_fin);
            Lista_Egreso.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_egreso_inventario(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Egreso.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
           // var model = bus_ing_inv.get_list(IdEmpresa, "-", IdSucursal, true, ViewBag.fecha_ini, ViewBag.fecha_fin);
            return PartialView("_GridViewPartial_egreso_inventario", model);
        }

        #endregion

        #region Cargar Unidad de medida
        public ActionResult CargarUnidadMedida()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            decimal IdProducto = (!string.IsNullOrEmpty(Request.Params["in_IdProducto"]) && Convert.ToDecimal(Request.Params["in_IdProducto"])!=0) ? Convert.ToDecimal(Request.Params["in_IdProducto"]) : 0;

            in_Producto_Info info_produto = bus_producto.get_info(IdEmpresa, IdProducto);
            string IdUnidadMedida = (info_produto == null) ? "" : info_produto.IdUnidadMedida_Consumo;

            return GridViewExtension.GetComboBoxCallbackResult(p =>
            {
                p.TextField = "Descripcion";
                p.ValueField = "IdUnidadMedida_equiva";
                p.ValueType = typeof(string);
                p.BindList(bus_UnidadMedidaEquivalencia.get_list_combo(IdUnidadMedida));
            });

        }
        #endregion

        #region Acciones

        public ActionResult Nuevo(int IdEmpresa = 0 )
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "EgresoInventario", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            in_parametro_Info i_param = bus_in_param.get_info(IdEmpresa);
            if (i_param == null)
                return RedirectToAction("Index");
            in_Ing_Egr_Inven_Info model = new in_Ing_Egr_Inven_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdEmpresa = IdEmpresa,
                cm_fecha = DateTime.Now,
                signo = "-",
                IdMovi_inven_tipo = i_param.P_IdMovi_inven_tipo_default_egr,
                IdMotivo_Inv = i_param.IdMotivo_Inv_egreso
            };
            List_in_Ing_Egr_Inven_det.set_list(new List<in_Ing_Egr_Inven_det_Info>(), model.IdTransaccionSession);
            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(in_Ing_Egr_Inven_Info model)
        {            
            model.lst_in_Ing_Egr_Inven_det = List_in_Ing_Egr_Inven_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                cargar_combos(model);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_ing_inv.guardarDB(model,"-"))
            {
                cargar_combos(model);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdMovi_inven_tipo = model.IdMovi_inven_tipo, IdNumMovi = model.IdNumMovi, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdSucursal = 0, int IdMovi_inven_tipo = 0, decimal IdNumMovi = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            bus_ing_inv = new in_Ing_Egr_Inven_Bus();
            bus_det_ing_inv = new in_Ing_Egr_Inven_det_Bus();
            in_Ing_Egr_Inven_Info model = bus_ing_inv.get_info(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "EgresoInventario", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            else
            {
                if(model.Estado == "A" && model.EstadoAprobacion == "APRO" && model.FechaDespacho==null)
                {
                    info.Modificar = false;
                }
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            model.lst_in_Ing_Egr_Inven_det = bus_det_ing_inv.get_list(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            List_in_Ing_Egr_Inven_det.set_list(model.lst_in_Ing_Egr_Inven_det, model.IdTransaccionSession);
            cargar_combos(model);
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cm_fecha, cl_enumeradores.eModulo.INV, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }

            if (model.IdEstadoAproba == "APRO" || model.IdEstadoAproba == "REP")
            {
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdSucursal = 0, int IdMovi_inven_tipo = 0, decimal IdNumMovi = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "EgresoInventario", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            bus_ing_inv = new in_Ing_Egr_Inven_Bus();
            bus_det_ing_inv = new in_Ing_Egr_Inven_det_Bus();
            in_Ing_Egr_Inven_Info model = bus_ing_inv.get_info(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            if (model == null)
                return RedirectToAction("Index");
            model.lst_in_Ing_Egr_Inven_det = bus_det_ing_inv.get_list(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            List_in_Ing_Egr_Inven_det.set_list(model.lst_in_Ing_Egr_Inven_det,model.IdTransaccionSession);
            cargar_combos(model);

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cm_fecha, cl_enumeradores.eModulo.INV, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }

            if (model.IdEstadoAproba == "APRO" || model.IdEstadoAproba == "REP")
            {
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(in_Ing_Egr_Inven_Info model)
        {
            model.lst_in_Ing_Egr_Inven_det = List_in_Ing_Egr_Inven_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                cargar_combos(model);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuarioUltModi = SessionFixed.IdUsuario;
            if (!bus_ing_inv.modificarDB(model))
            {
                cargar_combos(model);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdMovi_inven_tipo = model.IdMovi_inven_tipo, IdNumMovi = model.IdNumMovi, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0 , int IdSucursal = 0, int IdMovi_inven_tipo = 0, decimal IdNumMovi = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "EgresoInventario", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            bus_ing_inv = new in_Ing_Egr_Inven_Bus();
            bus_det_ing_inv = new in_Ing_Egr_Inven_det_Bus();
            in_Ing_Egr_Inven_Info model = bus_ing_inv.get_info(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            if (model == null)
                return RedirectToAction("Index");
            model.lst_in_Ing_Egr_Inven_det = bus_det_ing_inv.get_list(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            List_in_Ing_Egr_Inven_det.set_list(model.lst_in_Ing_Egr_Inven_det, model.IdTransaccionSession);
            cargar_combos(model);
            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (!bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cm_fecha, cl_enumeradores.eModulo.INV, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(in_Ing_Egr_Inven_Info model)
        {
            model.lst_in_Ing_Egr_Inven_det = List_in_Ing_Egr_Inven_det.get_list(model.IdTransaccionSession);
            if (!bus_periodo.ValidarFechaTransaccion(model.IdEmpresa, model.cm_fecha, cl_enumeradores.eModulo.INV, model.IdSucursal, ref mensaje))
            {
                cargar_combos(model);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdusuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_ing_inv.anularDB(model))
            {
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
         
        #region cargar combo y validaciones
        private void cargar_combos_detalle()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            in_UnidadMedida_Bus bus_unidad = new in_UnidadMedida_Bus();
            var lst_unidad = bus_unidad.get_list(false);
            ViewBag.lst_unidad = lst_unidad;
        }

        private bool validar(in_Ing_Egr_Inven_Info i_validar, ref string msg)
        {
            if (i_validar.lst_in_Ing_Egr_Inven_det.Count == 0)
            {
                mensaje = "Debe ingresar al menos un producto";
                return false;
            }
            if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.cm_fecha, cl_enumeradores.eModulo.INV, i_validar.IdSucursal, ref msg))
            {
                return false;
            }

            #region ValidarExisteProductoxBodega
            var param = bus_in_param.get_info(i_validar.IdEmpresa);
            mensaje = bus_producto_x_bodega.ValidarProductoPorBodega(new List<in_producto_x_tb_bodega_Info>(i_validar.lst_in_Ing_Egr_Inven_det.Select(q => new in_producto_x_tb_bodega_Info
            {
                IdEmpresa = i_validar.IdEmpresa,
                IdSucursal = i_validar.IdSucursal,
                IdBodega = i_validar.IdBodega ?? 0,
                IdProducto = q.IdProducto,
                pr_descripcion = q.pr_descripcion
            }).ToList()), (param.ValidarCtaCbleTransacciones ?? false));
            if (!string.IsNullOrEmpty(mensaje))
                return false;
            #endregion

            #region ValidarStock
            var lst_validar = i_validar.lst_in_Ing_Egr_Inven_det.GroupBy(q => new { q.IdProducto, q.pr_descripcion, q.tp_ManejaInven, q.se_distribuye }).Select(q => new in_Producto_Stock_Info
            {
                IdEmpresa = i_validar.IdEmpresa,
                IdSucursal = i_validar.IdSucursal,
                IdBodega = i_validar.IdBodega ?? 0,
                IdProducto = q.Key.IdProducto,
                pr_descripcion = q.Key.pr_descripcion,
                tp_manejaInven = q.Key.tp_ManejaInven,
                SeDestribuye = q.Key.se_distribuye,

                Cantidad = q.Sum(v => v.dm_cantidad_sinConversion),
                CantidadAnterior = q.Sum(v => v.CantidadAnterior),
            }).ToList();

            if (!bus_producto.validar_stock(lst_validar, ref msg))
            {
                return false;
            }
            #endregion

            #region ValidarCentroCosto
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ct_parametro_Bus bus_parametro = new ct_parametro_Bus();
            var info_ct_parametro = bus_parametro.get_info(IdEmpresa);

            if (i_validar.lst_in_Ing_Egr_Inven_det.Count > 0)
            {
                if (info_ct_parametro.EsCentroCostoObligatorio == true)
                {
                    foreach (var item in i_validar.lst_in_Ing_Egr_Inven_det)
                    {
                        if (item.IdCentroCosto== "" || item.IdCentroCosto == null)
                        {
                            mensaje = "Debe seleccionar el centro de costo para los items del detalle";
                            return false;
                        }
                    }
                }
            }
            #endregion

            return true;
        }
        private void cargar_combos(in_Ing_Egr_Inven_Info model)
        {
            in_movi_inven_tipo_Bus bus_tipo = new in_movi_inven_tipo_Bus();
            var lst_tipo = bus_tipo.get_list(model.IdEmpresa,"-", false);
            ViewBag.lst_tipo = lst_tipo;

            in_Motivo_Inven_Bus bus_motivo = new in_Motivo_Inven_Bus();
            var lst_motivo = bus_motivo.get_list(model.IdEmpresa, cl_enumeradores.eTipoIngEgr.EGR.ToString(), false);
            ViewBag.lst_motivo = lst_motivo;

            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.GetList(model.IdEmpresa, SessionFixed.IdUsuario, false);
            ViewBag.lst_sucursal = lst_sucursal;

            tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
            var lst_bodega = bus_bodega.get_list(model.IdEmpresa, model.IdSucursal, false);
            ViewBag.lst_bodega = lst_bodega;
        }
        #endregion

        #region funciones del detalle
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] in_Ing_Egr_Inven_det_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (info_det != null)
            {
                if (info_det.IdProducto != 0)
                {
                    in_Producto_Info info_producto = bus_producto.get_info(IdEmpresa, info_det.IdProducto);
                    if (info_producto != null)
                    {
                        info_det.pr_descripcion = info_producto.pr_descripcion_combo;
                        info_det.tp_ManejaInven = info_producto.tp_ManejaInven;
                        info_det.se_distribuye = info_producto.se_distribuye;
                        info_det.mv_costo_sinConversion = bus_prod_x_bod.get_ultimo_costo(IdEmpresa, Convert.ToInt32(SessionFixed.IdSucursalInv), Convert.ToInt32(SessionFixed.IdBodegaInv), info_det.IdProducto, DateTime.Now.Date);
                    }
                }
            }
            if (info_det.dm_cantidad_sinConversion>0 && info_det.IdProducto!=0)
            List_in_Ing_Egr_Inven_det.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_egr_inv_det", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] in_Ing_Egr_Inven_det_Info info_det)
        {            
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (info_det != null)
            {
                if (info_det.IdProducto != 0)
                {
                    in_Producto_Info info_producto = bus_producto.get_info(IdEmpresa, info_det.IdProducto);
                    if (info_producto != null)
                    {
                        info_det.pr_descripcion = info_producto.pr_descripcion_combo;
                        info_det.tp_ManejaInven = info_producto.tp_ManejaInven;
                        info_det.se_distribuye = info_producto.se_distribuye;
                        info_det.mv_costo_sinConversion = bus_prod_x_bod.get_ultimo_costo(IdEmpresa, Convert.ToInt32(SessionFixed.IdSucursalInv), Convert.ToInt32(SessionFixed.IdBodegaInv), info_det.IdProducto, DateTime.Now.Date);
                    }
                }
            }
                            
            if (info_det.dm_cantidad_sinConversion > 0)
                List_in_Ing_Egr_Inven_det.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_egr_inv_det", model);
        }

        public ActionResult EditingDelete(int Secuencia)
        {
            List_in_Ing_Egr_Inven_det.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_egr_inv_det", model);
        }
        #endregion

        #region Json
        public JsonResult SetPrecioProductoEgreso(int IdSucursal=0, int IdBodega=0, decimal IdProducto = 0)
        {
            in_producto_x_tb_bodega_Costo_Historico_Bus bus_producto_costo = new in_producto_x_tb_bodega_Costo_Historico_Bus();
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var fecha = DateTime.Now.Date;
            var resultado = bus_producto_costo.get_ultimo_costo(IdEmpresa, IdSucursal, IdBodega, IdProducto, fecha);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class in_Egreso_Inven_List
    {
        string Variable = "in_Egreso_Inven_Info";
        public List<in_Ing_Egr_Inven_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_Ing_Egr_Inven_Info> list = new List<in_Ing_Egr_Inven_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_Ing_Egr_Inven_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_Ing_Egr_Inven_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}
