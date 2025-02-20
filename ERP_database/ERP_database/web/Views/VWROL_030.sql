﻿CREATE VIEW web.VWROL_030
AS
SELECT        dbo.ro_rol.IdEmpresa, dbo.ro_rol.IdRol, dbo.ro_rol.IdSucursal, dbo.ro_rol.IdNominaTipo, dbo.ro_rol.IdNominaTipoLiqui, dbo.ro_rol.IdPeriodo, dbo.ro_rol_detalle.IdEmpleado, dbo.ro_rol_detalle.IdRubro, 
                         dbo.ro_cargo.ca_descripcion, dbo.tb_persona.pe_cedulaRuc, dbo.tb_persona.pe_nombreCompleto, dbo.ro_rubro_tipo.rub_codigo, dbo.ro_rubro_tipo.ru_codRolGen, dbo.ro_rubro_tipo.ru_orden, dbo.ro_rubro_tipo.ru_descripcion, 
                         dbo.ro_rol_detalle.Orden, dbo.ro_rol_detalle.Valor
FROM            dbo.ro_empleado INNER JOIN
                         dbo.ro_rol_detalle ON dbo.ro_empleado.IdEmpresa = dbo.ro_rol_detalle.IdEmpresa AND dbo.ro_empleado.IdEmpleado = dbo.ro_rol_detalle.IdEmpleado INNER JOIN
                         dbo.tb_persona ON dbo.ro_empleado.IdPersona = dbo.tb_persona.IdPersona LEFT OUTER JOIN
                         dbo.ro_rubro_tipo ON dbo.ro_rol_detalle.IdEmpresa = dbo.ro_rubro_tipo.IdEmpresa AND dbo.ro_rol_detalle.IdRubro = dbo.ro_rubro_tipo.IdRubro RIGHT OUTER JOIN
                         dbo.ro_rol ON dbo.ro_rol_detalle.IdEmpresa = dbo.ro_rol.IdEmpresa AND dbo.ro_rol_detalle.IdRol = dbo.ro_rol.IdRol LEFT OUTER JOIN
                         dbo.ro_cargo ON dbo.ro_empleado.IdEmpresa = dbo.ro_cargo.IdEmpresa AND dbo.ro_empleado.IdCargo = dbo.ro_cargo.IdCargo
GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWROL_030';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'idth = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWROL_030';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[86] 4[5] 2[3] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ro_rol"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 315
               Right = 229
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ro_rol_detalle"
            Begin Extent = 
               Top = 9
               Left = 323
               Bottom = 310
               Right = 511
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ro_empleado"
            Begin Extent = 
               Top = 0
               Left = 668
               Bottom = 305
               Right = 957
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "tb_persona"
            Begin Extent = 
               Top = 0
               Left = 1024
               Bottom = 295
               Right = 1255
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ro_cargo"
            Begin Extent = 
               Top = 277
               Left = 990
               Bottom = 407
               Right = 1207
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ro_rubro_tipo"
            Begin Extent = 
               Top = 52
               Left = 555
               Bottom = 453
               Right = 773
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         W', @level0type = N'SCHEMA', @level0name = N'web', @level1type = N'VIEW', @level1name = N'VWROL_030';

