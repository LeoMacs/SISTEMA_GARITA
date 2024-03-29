USE [master]
GO
/****** Object:  Database [IMUDESA]    Script Date: 08/02/2022 09:40:55 p.m. ******/
CREATE DATABASE [IMUDESA]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'IMUDESA', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS2012\MSSQL\DATA\IMUDESA.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'IMUDESA_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS2012\MSSQL\DATA\IMUDESA_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [IMUDESA] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IMUDESA].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IMUDESA] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [IMUDESA] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [IMUDESA] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [IMUDESA] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [IMUDESA] SET ARITHABORT OFF 
GO
ALTER DATABASE [IMUDESA] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [IMUDESA] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [IMUDESA] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [IMUDESA] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [IMUDESA] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [IMUDESA] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [IMUDESA] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [IMUDESA] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [IMUDESA] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [IMUDESA] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [IMUDESA] SET  DISABLE_BROKER 
GO
ALTER DATABASE [IMUDESA] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [IMUDESA] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [IMUDESA] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [IMUDESA] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [IMUDESA] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [IMUDESA] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [IMUDESA] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [IMUDESA] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [IMUDESA] SET  MULTI_USER 
GO
ALTER DATABASE [IMUDESA] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [IMUDESA] SET DB_CHAINING OFF 
GO
ALTER DATABASE [IMUDESA] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [IMUDESA] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [IMUDESA]
GO
/****** Object:  StoredProcedure [dbo].[asistenciaTrabajador]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[asistenciaTrabajador]
 @Fechadesde Date,
 @Fechahasta Date,
 @Idtrabajador int
AS
BEGIN
 
SELECT idcontrol,FORMAT(fechaentrada,'dd-MM-yyyy') as fechaentrada,horaentrada,FORMAT(fechasalida,'dd-MM-yyyy') as fechasalida ,horasalida,control.idtrabajador,
nombre,apellidoPaterno,apellidoMaterno,dni  FROM control INNER JOIN 
trabajadorcuenta ON control.idtrabajador = trabajadorcuenta.idtrabajador 
 where (fechaentrada between @Fechadesde and @Fechahasta) and 
control.idtrabajador=@Idtrabajador order by fechaentrada desc

END


GO
/****** Object:  StoredProcedure [dbo].[ListaAsistenciasHoyGarita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListaAsistenciasHoyGarita]
	
AS
	SELECT FORMAT(asistencia.fecha,'dd-MM-yyyy') as fecha,asistencia.horainicio,asistencia.horasalida,trabajador.idtrabajador,trabajador.nombre,trabajador.dni,empresa.nombre as empresa FROM asistencia 
            INNER JOIN trabajador ON asistencia.idtrabajador = trabajador.idtrabajador 
            INNER JOIN empresa ON empresa.idempresa = trabajador.idempresa 
            WHERE asistencia.fecha=(SELECT CONVERT (date, SYSDATETIME())) order by horainicio desc;
RETURN 0

GO
/****** Object:  StoredProcedure [dbo].[listarAsistenciaTrabajadoresxCuenta]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[listarAsistenciaTrabajadoresxCuenta] 
@fechadesde Date,
@fechahasta Date,
@cuenta Varchar(50)
AS
 SET NOCOUNT ON; 
BEGIN
 
 SELECT nombre+' '+apellidoPaterno+' '+apellidoMaterno as trabajador,idcontrol,FORMAT(fechaentrada ,'dd-MM-yyyy') as fechaentrada,horaentrada,FORMAT(fechasalida ,'dd-MM-yyyy') as fechasalida,horasalida,
 control.idtrabajador,dni  FROM control INNER JOIN 
trabajadorcuenta ON control.idtrabajador = trabajadorcuenta.idtrabajador 
 where (fechaentrada between @fechadesde and  @fechahasta) and tipocuenta=@cuenta and activo=1 order by fechaentrada desc;
  
END


GO
/****** Object:  StoredProcedure [dbo].[ListarCitasHoyGarita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListarCitasHoyGarita]
	
AS
	select idcita,FORMAT(fecha,'dd-MM-yyyy') as fecha, hora, nombre, dni, encargada, destino, empresa_proveniente,observacion, idcitador  from cita where fecha=(SELECT CONVERT (date, SYSDATETIME())) order by hora desc;
RETURN 0

GO
/****** Object:  StoredProcedure [dbo].[listarCitasHoyXUsuarioGarita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[listarCitasHoyXUsuarioGarita]

@idcitador int

AS
 SET NOCOUNT ON; 
BEGIN
 
 select idcita,FORMAT(fecha,'dd-MM-yyyy') as fecha,hora,nombre,dni,encargada,destino,empresa_proveniente,observacion,idcitador  from cita where fecha=(SELECT CONVERT (date, SYSDATETIME())) 
 and idcitador=@idcitador order by hora desc;
END


GO
/****** Object:  StoredProcedure [dbo].[listarCitasxFiltrosGarita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[listarCitasxFiltrosGarita] 
@nombre varchar(50),   
@encargada varchar(50),
@destino varchar(50),
@fechai date,
@fechaf date,
@dni varchar(10)

AS
 SET NOCOUNT ON; 
BEGIN
 
SELECT idcita,FORMAT(fecha,'dd-MM-yyyy') as fecha,hora,nombre,dni,encargada,destino,empresa_proveniente,observacion,idcitador FROM cita
 WHERE nombre LIKE '%' + @nombre + '%' and encargada LIKE '%' + @encargada + '%' and destino LIKE '%' + @destino + '%' and (fecha between @fechai  and   @fechaf) and dni LIKE '%' + @dni + '%' order by fecha desc;
END


GO
/****** Object:  StoredProcedure [dbo].[ListarSegurosGarita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListarSegurosGarita]
	
AS
	SELECT  empresa.nombre as empresa, seguro.nombre, nrosalud, nropoliza, FORMAT(fechainicio,'dd-MM-yyyy') as fechainicio, FORMAT(fechafin,'dd-MM-yyyy') as fechafin  FROM seguro 
	INNER JOIN empresa   ON seguro.idempresa = empresa.idempresa where empresa.idempresa>'0';
RETURN 0

GO
/****** Object:  StoredProcedure [dbo].[ListarTrabajadoresGarita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListarTrabajadoresGarita]
	
AS
	SELECT  trabajador.nombre, dni, cargo, area,habilitado, empresa.nombre as empresa,FORMAT(seguro.fechafin,'dd-MM-yyyy') as fechafin   FROM trabajador
	 INNER JOIN empresa ON trabajador.idempresa = empresa.idempresa
	 INNER JOIN seguro ON seguro.idempresa = empresa.idempresa
	 where trabajador.habilitado='1' order by empresa;
RETURN 0

GO
/****** Object:  StoredProcedure [dbo].[listarTrabajadoresxCuenta]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[listarTrabajadoresxCuenta] 
@cuenta Varchar(50)
AS
 SET NOCOUNT ON; 
BEGIN
 
 SELECT *
    FROM trabajadorcuenta  
    WHERE tipocuenta=@cuenta  and activo=1; 
END


GO
/****** Object:  StoredProcedure [dbo].[ListarVisitasHoyGarita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListarVisitasHoyGarita]
	
AS

	SELECT idvisitante,FORMAT(fecha,'dd-MM-yyyy') as fecha,horaentrada,horasalida,nombre,dni,destino,encargado,empresa_proveniente FROM visitante where fecha=(SELECT CONVERT (date, SYSDATETIME())) order by horaentrada desc;

RETURN 0

GO
/****** Object:  StoredProcedure [dbo].[sp_addseguro]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_addseguro]
@nombre varchar(50),
@nrosalud varchar(20),
@nropoliza varchar(20),
@fechainicio varchar(10),
@fechafin varchar(10),
@idempresa int
AS
 SET NOCOUNT OFF; 
BEGIN 
set @fechainicio=CONCAT(SUBSTRING(@fechainicio, 7, 4), '/',SUBSTRING(@fechainicio, 4, 2) , '/', SUBSTRING(@fechainicio, 1, 2));
set @fechafin=CONCAT(SUBSTRING(@fechafin, 7, 4), '/',SUBSTRING(@fechafin, 4, 2) , '/', SUBSTRING(@fechafin, 1, 2));
if @fechainicio<=@fechafin
	begin

		insert into seguro(nombre,nrosalud,nropoliza,fechainicio,fechafin,idempresa,usucreacion,feccreacion) 
		values (@nombre,@nrosalud,@nropoliza,@fechainicio,@fechafin,@idempresa,'MACS',dateadd(hour,1,getdate()));

		update empresa set conSeguro='SI',usumodificacion='MACS',
		fecmodificacion=dateadd(hour,1,getdate())
		 where idempresa=@idempresa;
	end
END

GO
/****** Object:  StoredProcedure [dbo].[sp_del_cita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[sp_del_cita]
@idcita int=0,
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT

AS

	select  @nResultado=0

BEGIN TRANSACTION

		update cita set bEstado=0,dFecMod=dbo.fn_get_fechaCorregida(@codPais),
		cUsuMod=@user where idcita=@idcita
		select @nResultado=1
	
COMMIT TRANSACTION	
GO
/****** Object:  StoredProcedure [dbo].[sp_get_cargaVehicular]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_cargaVehicular]
@fechai Varchar(10)='01/01/1990',
@fechaf Varchar(10)='01/01/1990',
@dni Varchar(10)='',
@nombre Varchar(50)='',
@licencia Varchar(15)='',
@placa Varchar(15)='',
@proveniente Varchar(50)='',
@destino Varchar(50)='',
@nSucursal int=0
AS

	select v.idCargaVehicular as ID, v.nSucursal, v.idPersona,
	(p.cApPaterno+' '+ p.cApMaterno+' ' +p.cNombres) as persona,
	p.cDocPer as dni, 
	FORMAT(v.dFecha,'dd/MM/yyyy') as fecha,
	v.dHoraEntrada as horaentrada,
	v.dHoraSalida as horasalida,
	----------------------------------
	(case ltrim(rtrim(v.dHoraSalida))
	when '' then ''
	else 'true'
	end) as hasalido, 
	----------------------------------
	v.cLicencia as licencia,
	v.cEmpresaPvnte as origen,
	v.idEmpresaDestino as iddestino,
	e.cNombre as destino,
	v.cPlaca as placa,
	v.cNumContenedor as numContenedor,
	v.bUnidad,
	v.bCarreta,
	v.cCodCarreta as codCarreta,
	v.cTipo as tipo,
	v.cEstadoCarga as estcarga,
	v.cPrecintoGuia as precinto,
	v.dFecReg as fecReg,
	v.cUsuReg as usuReg
	 from cargaVehicular v 
	inner join persona p on p.idPersona=v.idPersona
	inner join empresa e on e.idempresa=v.idEmpresaDestino
	where v.nSucursal=@nSucursal
	and v.bEstado=1
	and FORMAT(v.dFecha,'dd/MM/yyyy') between @fechai and @fechaf
		and p.cDocPer  like '%'+@dni+'%'
		and (p.cApPaterno + ' '+p.cApMaterno + ' '+p.cNombres) like '%'+@nombre+'%'
		and v.cLicencia like '%'+@licencia+'%'
		and v.cPlaca like '%'+@placa+'%'
		and v.cEmpresaPvnte like '%'+@proveniente+'%'
		and e.cNombre like '%'+@destino+'%'
		order by fecha desc, horaentrada desc

RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[sp_get_cargaVehicularbyid]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_cargaVehicularbyid]
@id int=0,
@codPais varchar(5)
AS
	if @id=0
		begin
			select  idCargaVehicular=0,idPersona=0, nombres='', apPaterno='' ,apMaterno='' ,
			dni='',fecha=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy'),
			horaentrada=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'HH:mm'),horasalida='',
			licencia='',empresaPvnte='',idDestino=4,placa='', numContenedor='',bUnidad=0,
			bCarreta=0, codcarreta='', tipo='20',estcarga='V',precinto=''
		end
	else
		begin
				select v.idCargaVehicular,v.idPersona,p.cNombres as  nombres,p.cApPaterno as apPaterno,
				p.cApMaterno as apMaterno,p.cDocPer as dni, FORMAT(v.dFecha,'dd/MM/yyyy') as fecha,
				v.dHoraEntrada as horaentrada,v.dHoraSalida as horasalida,
				v.cLicencia as licencia,v.cEmpresaPvnte as empresaPvnte,v.idEmpresaDestino as idDestino,
				v.cPlaca as placa,v.cNumContenedor as numContenedor,v.bUnidad,
				v.bCarreta, v.cCodCarreta as codcarreta,v.cTipo as tipo,v.cEstadoCarga as estcarga, 
				v.cPrecintoGuia as precinto
				from cargavehicular v
				inner join persona p on p.idPersona=v.idPersona
				where v.idCargaVehicular=@id
		end
	
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[sp_get_cargaVehicularHoy]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_cargaVehicularHoy]
@nSucursal int=0,
@codPais varchar(3)	
AS

	select v.idCargaVehicular as ID, v.nSucursal, v.idPersona,
	(p.cApPaterno+' '+ p.cApMaterno+' ' +p.cNombres) as persona,
	p.cDocPer as dni, 
	FORMAT(v.dFecha,'dd/MM/yyyy') as fecha,
	v.dHoraEntrada as horaentrada,
	v.dHoraSalida as horasalida,
	----------------------------------
	(case ltrim(rtrim(v.dHoraSalida))
	when '' then ''
	else 'true'
	end) as hasalido, 
	----------------------------------
	v.cLicencia as licencia,
	v.cEmpresaPvnte as origen,
	v.idEmpresaDestino as iddestino,
	e.cNombre as destino,
	v.cPlaca as placa,
	v.cNumContenedor as numContenedor,
	v.bUnidad,
	v.bCarreta,
	v.cCodCarreta as codCarreta,
	v.cTipo as tipo,
	v.cEstadoCarga as estcarga,
	v.cPrecintoGuia as precinto,
	v.bEstado,
	v.dFecReg as fecReg,
	v.cUsuReg as usuReg
	 from cargaVehicular v 
	inner join persona p on p.idPersona=v.idPersona
	inner join empresa e on e.idempresa=v.idEmpresaDestino
	where v.nSucursal=@nSucursal
	and FORMAT(v.dFecha,'dd/MM/yyyy')=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy') 
	
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[sp_get_citabyid]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_citabyid]
@id int=0,
@codPais varchar(5)
AS
	if @id=0
		begin
			select idcita=0,idPersona=0, nombres='', apPaterno='' ,apMaterno='' ,
			persona='',
			dni='',fecha=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy'),
			hora=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'HH:mm'),
			'' as destino,
			encargado='',empresaPvnte='',idDestino=4,observacion='' 
			
		end
	else
		begin
				select c.idcita,c.idPersona,p.cNombres as  nombres,p.cApPaterno as apPaterno,
				(p.cApPaterno+' '+ p.cApMaterno+' ' +p.cNombres) as persona,
				p.cApMaterno as apMaterno,p.cDocPer as dni, FORMAT(c.dFecha,'dd/MM/yyyy') as fecha,
				c.cHora as hora,
				c.cEncargado as encargado,c.cEmpresaPvnte as empresaPvnte,c.idDestino,
				(select e.cNombre from empresa e where e.idEmpresa=c.idDestino) as destino,
				c.cObservacion as observacion
				from cita c
				inner join persona p on p.idPersona=c.idPersona
				where c.idcita=@id
		end
	
RETURN 0

GO
/****** Object:  StoredProcedure [dbo].[sp_get_citas]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_citas]
@fechai Varchar(10)='01/01/1990',
@fechaf Varchar(10)='01/01/1990',
@nombre Varchar(50)='',
@dni Varchar(10)='',
@destino Varchar(50)='',
@encargado Varchar(50)='',
@empresa_proveniente Varchar(50)='',
@nSucursal int=0,
@idusuario int

AS
 SET NOCOUNT ON;
declare
@perfil varchar(50)  

select @perfil=a.cNombre 
 from usuario u 
 inner join  area a on u.idArea=a.idArea
 where u.idUsuario=@idusuario
 
 if(@perfil='SEGURIDAD' or @perfil='SISTEMAS')
	BEGIN
	select c.idcita,
	FORMAT(c.dFecha,'dd-MM-yyyy') as fecha,
	c.cHora as hora,
	(p.cApPaterno+' '+ p.cApMaterno+' ' +p.cNombres) as persona,
	p.cDocPer as dni,
	e.cNombre as destino,
	c.idDestino,
	c.cEncargado as encargado,
	c.cEmpresaPvnte as empresaPvnte,c.cObservacion as observacion,idcitador,
	(case c.bAtendido when 1 then 'true' else '' end) as haatendido,
	(case c.bAtendido when 1 then 'SE PRESENTÓ' else 'NO SE PRESENTÓ' end) as estado,
	(case c.bAtendido when 1 then 'green' else 'red' end) as colorEstado,
	(select cUsuNombre from usuario where idusuario=c.idCitador) as usuario
	from cita c inner join persona p on c.idPersona=p.idPersona
	inner join empresa e on e.idEmpresa=c.idDestino
	where 
	(c.dFecha between  @fechai and @fechaf) and
	 p.cDocPer  like '%'+@dni+'%'
	and (p.cApPaterno + ' '+p.cApMaterno + ' '+p.cNombres) like '%'+@nombre+'%'
	and e.cNombre like '%'+@destino+'%'
	and c.cEncargado like '%'+@encargado+'%'
	and c.bEstado=1 and p.bEstado=1
	order by dFecha desc, cHora desc
	END
 else
	BEGIN
	select c.idcita,
	FORMAT(c.dFecha,'dd-MM-yyyy') as fecha,
	c.cHora as hora,
	(p.cApPaterno+' '+ p.cApMaterno+' ' +p.cNombres) as persona,
	p.cDocPer as dni,
	e.cNombre as destino,
	c.idDestino,
	c.cEncargado as encargado,
	c.cEmpresaPvnte as empresaPvnte,c.cObservacion as observacion,idcitador,
	(case c.bAtendido when 1 then 'true' else '' end) as haatendido,
	(case c.bAtendido when 1 then 'SE PRESENTÓ' else 'NO SE PRESENTÓ' end) as estado,
	(case c.bAtendido when 1 then 'green' else 'red' end) as colorEstado,
	(select cUsuNombre from usuario where idusuario=c.idCitador) as usuario
	from cita c inner join persona p on c.idPersona=p.idPersona
	inner join empresa e on e.idEmpresa=c.idDestino
	where (c.dFecha between  @fechai and @fechaf)
	and p.cDocPer  like '%'+@dni+'%'
	and (p.cApPaterno + ' '+p.cApMaterno + ' '+p.cNombres) like '%'+@nombre+'%'
	and e.cNombre like '%'+@destino+'%'
	and c.cEncargado like '%'+@encargado+'%'
	AND C.idcitador=@idusuario
	and c.bEstado=1 and p.bEstado=1
	order by dFecha desc, cHora desc
	END

GO
/****** Object:  StoredProcedure [dbo].[sp_get_citasHoy]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_citasHoy]
@idusuario int,
@nSucursal int=0,
@codPais varchar(3)	

AS
 SET NOCOUNT ON;
declare
@perfil varchar(50)  

select @perfil=a.cNombre 
 from usuario u 
 inner join  area a on u.idArea=a.idArea
 where u.idUsuario=@idusuario

BEGIN
 
 if(@perfil='SEGURIDAD' or @perfil='SISTEMAS')
	select c.idcita,
	FORMAT(c.dFecha,'dd-MM-yyyy') as fecha,
	c.cHora as hora,
	(p.cApPaterno+' '+ p.cApMaterno+' ' +p.cNombres) as persona,
	p.cDocPer as dni,
	(select e.cNombre from empresa e where e.idEmpresa=c.idDestino) as destino,
	c.idDestino,
	c.cEncargado as encargado,
	c.cEmpresaPvnte as empresaPvnte,c.cObservacion as observacion,idcitador,
	(case c.bAtendido when 1 then 'true' else '' end) as haatendido,
	(select cUsuNombre from usuario where idusuario=c.idCitador) as usuario,
	'true' as noedit,
	'true' as nodelete,
	(case c.bAtendido when 1 then 'true' else '' end) as noconfirm
	from cita c inner join persona p on c.idPersona=p.idPersona
	where FORMAT(c.dFecha,'dd/MM/yyyy')=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy') 
	and c.bEstado=1 and p.bEstado=1
	order by c.cHora desc;

 else
	select c.idcita,
	FORMAT(c.dFecha,'dd-MM-yyyy') as fecha,
	c.cHora as hora,
	(p.cApPaterno+' '+ p.cApMaterno+' ' +p.cNombres) as persona,
	p.cDocPer as dni,
	(select e.cNombre from empresa e where e.idEmpresa=c.idDestino) as destino,
	c.idDestino,
	c.cEncargado as encargado,
	c.cEmpresaPvnte as empresaPvnte,c.cObservacion as observacion,idcitador,
	(case c.bAtendido when 1 then 'true' else '' end) as haatendido,
	(select cUsuNombre from usuario where idusuario=c.idCitador) as usuario,
	(case c.bAtendido when 1 then 'true' else '' end) as noedit,
	(case c.bAtendido when 1 then 'true' else '' end) as nodelete,
	'true' as noconfirm
	from cita c inner join persona p on c.idPersona=p.idPersona
	where FORMAT(c.dFecha,'dd/MM/yyyy')=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy') 
	and c.bEstado=1 and c.bOculto=0
	and c.idcitador=@idusuario
	and p.bEstado=1
	order by c.cHora desc;

END

GO
/****** Object:  StoredProcedure [dbo].[sp_get_Empresabyid]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_Empresabyid]
@idempresa int  
AS

BEGIN 

SELECT e.idEmpresa,e.cRuc as ruc ,e.cNombre as nombre,
e.cDescripcion as descripcion, dFecReg as fecReg,
cUsuReg as usuReg
from empresa e
where e.idempresa=@idempresa;

END


GO
/****** Object:  StoredProcedure [dbo].[sp_get_Empresas]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_Empresas] 
@sucursal int
AS

BEGIN 

SELECT e.idEmpresa,e.cRuc as ruc ,e.cNombre as nombre,
e.cDescripcion as descripcion, dFecReg as fecReg,
cUsuReg as usuReg
 from empresa e where e.bEstado=1
 and e.nSucursal=@sucursal
 order by nombre asc;

END

GO
/****** Object:  StoredProcedure [dbo].[sp_get_EmpresasconSeleccion]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_EmpresasconSeleccion]
@idempresa int, 
@sucursal int
AS
 
BEGIN 
--SELECT e.idEmpresa,e.cRuc as ruc ,e.cNombre as nombre,
--e.cDescripcion as descripcion, 
--(case 
-- when e.idempresa=@idempresa then 'selected'
--	ELSE ''
--END	) estado,
--dFecReg as fecReg,
--cUsuReg as usuReg
-- from empresa e where e.bEstado=1
-- and e.nSucursal=@sucursal
-- order by nombre asc;
SELECT e.idEmpresa,e.cRuc as ruc ,e.cNombre as nombre,
e.cDescripcion as descripcion, 'selected' as estado,
dFecReg as fecReg,
cUsuReg as usuReg
 from empresa e where e.bEstado=1 and e.idempresa=@idempresa
 and e.nSucursal=@sucursal

 union

 SELECT e.idEmpresa,e.cRuc as ruc ,e.cNombre as nombre,
e.cDescripcion as descripcion, '' as estado,
dFecReg as fecReg,
cUsuReg as usuReg
 from empresa e where e.bEstado=1 and e.idempresa<>@idempresa
 and e.nSucursal=@sucursal
 --order by nombre asc;



END
GO
/****** Object:  StoredProcedure [dbo].[sp_get_indeseadobyIdDni]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_indeseadobyIdDni]
@id integer=0,
@dni varchar(10)='0000000000',
@modo varchar(3)='I'
AS
	if @modo='I'
	  BEGIN
		if @id= 0
			select idindeseado=0,idPersona=0,dni='',
			nombre='',apPaterno='',apMaterno='',
			persona='' , cargo='', empresaPvnte='',
			observacion='',fechaRegistro=FORMAT(GETDATE(),'dd/MM/yyyy'),
			usuario='' 
			
		else
			select i.idindeseado,p.idPersona,p.cDocPer as dni,
			p.cNombres as nombre, p.cApPaterno as apPaterno,
			p.cApMaterno as apMaterno,
			(p.cApPaterno+' '+p.cApMaterno+' '+p.cNombres) as persona,  
			i.cCargo as cargo, i.cEmpresaPvnte as empresaPvnte, i.cObservacion as observacion,
			FORMAT(i.dFecReg,'dd/MM/yyyy') as fechaRegistro,
			i.cUsuReg as usuario from indeseado i
			inner join persona p on p.idPersona=i.idPersona
			where i.idIndeseado=@id
			
	  END
	else --@modo='D'
		begin
			select i.idindeseado,p.idPersona,p.cDocPer as dni,
			p.cNombres as nombre, p.cApPaterno as apPaterno,
			p.cApMaterno as apMaterno,
			(p.cApPaterno+' '+p.cApMaterno+' '+p.cNombres) as persona,  
			i.cCargo as cargo, i.cEmpresaPvnte as empresaPvnte, i.cObservacion as observacion,
			FORMAT(i.dFecReg,'dd/MM/yyyy') as fechaRegistro,
			i.cUsuReg as usuario from indeseado i
			inner join persona p on p.idPersona=i.idPersona
			where i.idIndeseado=@id		
		end
	
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[sp_get_indeseados]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_indeseados]
AS
		begin
			select i.idindeseado,p.idPersona,p.cDocPer as dni,
			(p.cApPaterno+' '+p.cApMaterno+' '+p.cNombres) as persona,  
			i.cCargo as cargo, i.cEmpresaPvnte as empresaPvnte, i.cObservacion as observacion,
			FORMAT(i.dFecReg,'dd/MM/yyyy') as fechaRegistro,
			i.cUsuReg as usuario from indeseado i
			inner join persona p on p.idPersona=i.idPersona
			order by fechaRegistro desc
		
		end
	
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[sp_get_movimientobyid]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_movimientobyid]
@idMovimiento int=0,
@codPais varchar(5)
AS
	if @idMovimiento=0
		begin
			select  idMovimiento=0,idPersona=0, nombres='', apPaterno='' ,apMaterno='' ,
			dni='',fecha=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy'),
			horaentrada=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'HH:mm'),
			horasalida='',destino='',encargado='',empresaPvnte='',
			numAsignado='',observacion=''
		end
	else
		begin
				select m.idMovimiento,m.nSucursal,m.idPersona,
				p.cApPaterno as apPaterno,  p.cApMaterno as apMaterno,
				p.cNombres as  nombres,p.cDocPer as dni, 
				FORMAT(m.dFecha,'dd/MM/yyyy') as fecha,
				m.dHoraEntrada as horaentrada,
				m.dHoraSalida as horasalida,
				m.cDestino as destino,
				m.cEncargado as encargado,
				m.cEmpresaPvnte as empresaPvnte,
				m.nNumAsignado as numAsignado,
				m.cObservacion as observacion
				 from movimientoPersona m 
				inner join persona p on p.idPersona=m.idPersona
				where m.idMovimiento=@idMovimiento
		end
	
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[sp_get_movimientos]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_movimientos]
@fechai Varchar(10)='01/01/1990',
@fechaf Varchar(10)='01/01/1990',
@nombre Varchar(50)='',
@dni Varchar(10)='',
@destino Varchar(50)='',
@encargado Varchar(50)='',
@empresa_proveniente Varchar(50)='',
@nSucursal int=0,
@modoMov Varchar(5)='v'
AS

	if @modoMov='v'
	begin
		select m.idMovimiento,m.nSucursal,m.idPersona,
		(p.cApPaterno+' '+ p.cApMaterno+' ' +p.cNombres) as persona,
		p.cDocPer as dni, 
		FORMAT(m.dFecha,'dd/MM/yyyy') as fecha,
		m.dFecha,
		m.dHoraEntrada as horaentrada,
		m.dHoraSalida as horasalida,
		----------------------------------
		m.nNumAsignado as numAsignado,
		m.cDestino as destino,
		m.cEncargado as encargado,
		m.cEmpresaPvnte as empresaPvnte,
		m.cObservacion as observacion,
		m.cModoMov as modo
		 from movimientoPersona m 
		inner join persona p on p.idPersona=m.idPersona
		where (m.cModoMov='v' or m.cModoMov='c')
		and m.bEstado=1
		and m.nSucursal=@nSucursal
		--and FORMAT(m.dFecha,'dd/MM/yyyy') between @fechai and @fechaf
		and (m.dFecha between @fechai and @fechaf)
		and p.cDocPer  like '%'+@dni+'%'
		and (p.cApPaterno + ' '+p.cApMaterno + ' '+p.cNombres) like '%'+@nombre+'%'
		and cDestino like '%'+@destino+'%'
		and cEncargado like '%'+@encargado+'%'
		and cEmpresaPvnte like '%'+@empresa_proveniente+'%'
		order by dFecha desc, horaentrada desc
	end
	
	
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[sp_get_movimientosHoy]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_movimientosHoy]
@modoMov Varchar(5)='v',
@nSucursal int=0,
@codPais varchar(3)	
AS

	if @modoMov='v'
		begin
			select m.idMovimiento,m.nSucursal,m.idPersona,
			(p.cApPaterno+' '+ p.cApMaterno+' ' +p.cNombres) as persona,
			p.cDocPer as dni, 
			FORMAT(m.dFecha,'dd/MM/yyyy') as fecha,
			m.dHoraEntrada as horaentrada,
			m.dHoraSalida as horasalida,
			----------------------------------
			(case ltrim(rtrim(m.dHoraSalida))
			when '' then ''
			else 'true'
			end) as hasalido, 
			----------------------------------
			m.nNumAsignado as numAsignado,
			m.cDestino as destino,
			m.cEncargado as encargado,
			m.cEmpresaPvnte as empresaPvnte,
			m.cObservacion as observacion,
			m.cModoMov as modo,
			m.dFecMod as fecMod,
			m.cUsuMod as usuMod
			from movimientoPersona m 
			inner join persona p on p.idPersona=m.idPersona
			where 
			(m.cModoMov='v' or m.cModoMov='c')
			and nSucursal=@nSucursal
			and FORMAT(m.dFecha,'dd/MM/yyyy')=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy') 
		end
	else
		begin
			select m.idMovimiento,m.nSucursal,m.idPersona,
			(p.cApPaterno+' '+ p.cApMaterno+' ' +p.cNombres) as persona,
			p.cDocPer as dni, 
			FORMAT(m.dFecha,'dd/MM/yyyy') as fecha,
			m.dHoraEntrada as horaentrada,
			m.dHoraSalida as horasalida,
			----------------------------------
			(case ltrim(rtrim(m.dHoraSalida))
			when '' then ''
			else 'true'
			end) as hasalido, 
			----------------------------------
			m.nNumAsignado as numAsignado,
			m.cDestino as destino,
			m.cEncargado as encargado,
			m.cEmpresaPvnte as empresaPvnte,
			m.cObservacion as observacion,
			m.cModoMov as modo,
			m.dFecMod as fecMod,
			m.cUsuMod as usuMod
			from movimientoPersona m 
			inner join persona p on p.idPersona=m.idPersona
			where m.cModoMov=@modoMov
			and nSucursal=@nSucursal
			and FORMAT(m.dFecha,'dd/MM/yyyy')=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy') 
			
		end

	
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[sp_get_persona]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_persona]
@dni varchar(10)='',--modo 1
@idPersona int,--modo 2
@modo int
AS
declare @siExistePer int=0
	if @modo=1
		begin
		    select @siExistePer=COUNT(*) from persona where cDocPer=@dni;
			if @siExistePer<>0
				select idPersona, cDocPer as dni,cNombres as nombres,
				cApPaterno as apPaterno,cApMaterno as apMaterno
				from persona where cDocPer=@dni;
			else
				select idPersona=0, dni='',nombres='',apPaterno='',
				apMaterno=''
		end
	--else
	--	begin
	--			select m.idMovimiento,m.nSucursal,m.idPersona,
	--			p.cApPaterno as apPaterno,  p.cApMaterno as apMaterno,
	--			p.cNombres as  nombres,p.cDocPer as dni, 
	--			FORMAT(m.dFecha,'dd/MM/yyyy') as fecha,
	--			m.dHoraEntrada as horaentrada,
	--			m.dHoraSalida as horasalida,
	--			m.cDestino as destino,
	--			m.cEncargado as encargado,
	--			m.cEmpresaPvnte as empresaPvnte,
	--			m.nNumAsignado as numAsignado,
	--			m.cObservacion as observacion
	--			 from movimientoPersona m 
	--			inner join persona p on p.idPersona=m.idPersona
	--			where m.idMovimiento=@idMovimiento
	--	end
	
RETURN 0
GO
/****** Object:  StoredProcedure [dbo].[sp_get_Sucursales]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_Sucursales]	
AS
SET NOCOUNT ON;
begin
	SELECT  s.idsucursal,s.cNombre
	FROM sucursal s
	where s.bEstado=1;
end

GO
/****** Object:  StoredProcedure [dbo].[sp_get_Usuarios]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_Usuarios] 
AS
 SET NOCOUNT ON; 
BEGIN 

SELECT u.idusuario,u.nSucursal,s.cNombre,u.cUsuNombre, a.cNombre 
from usuario u 
inner join sucursal s on s.idSucursal=u.nSucursal
inner join area a on a.idArea=u.idArea
order by 3 asc

END


GO
/****** Object:  StoredProcedure [dbo].[sp_get_UsuarioxID]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_UsuarioxID] 
@idusuario INT=0
AS
 SET NOCOUNT ON; 
BEGIN 

SELECT u.idusuario,u.nSucursal,s.cNombre,u.cUsuNombre,
u.idArea, a.cNombre 
from usuario u 
inner join sucursal s on s.idSucursal=u.nSucursal
inner join area a on a.idArea=u.idArea
where u.idusuario=@idusuario

END


GO
/****** Object:  StoredProcedure [dbo].[Sp_getEmpDispForSeguro]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-----------------------------------------------

CREATE PROCEDURE [dbo].[Sp_getEmpDispForSeguro] 
AS
 SET NOCOUNT ON; 
BEGIN 

SELECT e.idempresa,e.nombre,e.descripcion from empresa e 
where e.conSeguro='NO';

END


GO
/****** Object:  StoredProcedure [dbo].[Sp_getPersonalxEmpresa]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_getPersonalxEmpresa]
@idempresa int  
AS
 SET NOCOUNT ON; 
BEGIN 

select t.idtrabajador,t.nombre, t.dni,t.cargo,t.area,
t.habilitado,t.idempresa, e.nombre empresa 
from trabajador t
inner join empresa e on e.idempresa=t.idempresa
where t.idempresa=@idempresa
order by 2 asc;

END


GO
/****** Object:  StoredProcedure [dbo].[Sp_getSCTRxEmpresa]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_getSCTRxEmpresa]
@idempresa int  
AS
 SET NOCOUNT ON; 
BEGIN 

select s.idseguro,s.nombre,s.nrosalud,s.nropoliza,
FORMAT(s.fechainicio,'dd/MM/yyyy') fechainicio,
FORMAT(s.fechafin,'dd/MM/yyyy') fechafin,e.nombre empresa,
s.idempresa  from seguro s
inner join empresa e on e.idempresa=s.idempresa
where s.idempresa=@idempresa;

END


GO
/****** Object:  StoredProcedure [dbo].[sp_getSeguros]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_getSeguros]	
AS
SET NOCOUNT ON;
declare
 @hoy date=dateadd(hour,1,getdate());
begin
	SELECT  e.idempresa,e.nombre empresa,s.idseguro, s.nombre, s.nrosalud, s.nropoliza, FORMAT(s.fechainicio,'dd-MM-yyyy') fechainicio, FORMAT(s.fechafin,'dd-MM-yyyy') fechafin,
	(case 
		when (@hoy>=s.fechainicio and  @hoy<=s.fechafin) then 'VIGENTE'
		else  'NO VIGENTE'
	end) estado,
	(case 
		when (@hoy>=s.fechainicio and  @hoy<=s.fechafin) then 'GREEN'
		else  'RED'
	end) colorEstado
	FROM seguro s, empresa e
	where e.idempresa>'0' and s.idempresa = e.idempresa and s.flagactivo=1;
end

GO
/****** Object:  StoredProcedure [dbo].[sp_getTrabajadores]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_getTrabajadores]
	
AS
SET NOCOUNT ON;
 
begin
	SELECT  t.idtrabajador,t.nombre, t.dni, t.cargo, t.area,t.habilitado,
	t.fecingreso,t.fecretiro, e.nombre  empresa,e.idempresa,
	(case 
		when (t.habilitado=1) then 'ACTIVO'
		else  'NO ACTIVO'
	end) estado,
	(case 
		when (t.habilitado=1) then 'GREEN'
		else  'RED'
	end) colorEstado,
	(case 
		when (t.habilitado=1) then 'checked'
		else  ''
	end) opcionsi,
	(case 
		when (t.habilitado=0) then 'checked'
		else  ''
	end) opcionno 
	FROM trabajador t, empresa e
	where t.idempresa = e.idempresa  
	 order by empresa,t.habilitado desc;	
end



GO
/****** Object:  StoredProcedure [dbo].[sp_iniciarSesion]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_iniciarSesion] 
@cuenta varchar(50)='',
@clave varchar(50)=''
AS
 SET NOCOUNT ON; 
declare
@ID int=0

select @ID=ISNULL(idUsuario,0) from usuario where rtrim(cUsuNombre)=RTRIM(@cuenta)
and rtrim(cUsuCtseña)=RTRIM(@clave)

if @ID<>0 
begin
	SELECT u.idusuario,u.nSucursal,s.cNombre,u.cUsuNombre,
	u.idArea, a.cNombre as area
	from usuario u 
	inner join sucursal s on s.idSucursal=u.nSucursal
	inner join area a on a.idArea=u.idArea
	where u.idusuario=@ID
END
else
begin
	select 0 as idusuario, '' as  nSucursal , '' as cNombre ,'' as cUsuNombre,
	0 as idArea, '' as area
end


GO
/****** Object:  StoredProcedure [dbo].[sp_ins_cargaVehicular]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ins_cargaVehicular]
@sucursal int=1,
@fecha varchar(10)='',
@horaEntrada varchar(10)='',
@horaSalida varchar(10)='',
@dni varchar(10)='',
@nombres varchar(50)='',
@apPaterno varchar(50)='',
@apMaterno varchar(50)='',
@licencia varchar(15)='',
@empresa_pvnte varchar(50)='',
@iddestino varchar(200)='',
@placa varchar(15)='',
@numContenedor varchar(15)='',
@bUnidad int=0,
@bCarreta int=0,
@codCarreta varchar(15)='',
@tipo varchar(5)='20',
@estado varchar(5)='V',
@precinto varchar(50)='',
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT

AS
 --SET NOCOUNT ON; --cuenta registros afectados cuando está en off
BEGIN 
declare @idPersona int,
@siExiste int,
@siUsadoPer int

	select @siExiste=count(*) from persona where cDocPer=@dni and bEstado=1

	select @siUsadoPer=count(*) from cargavehicular c
	inner join persona p on p.idPersona =c.idPersona 
	where p.cDocPer=@dni and c.bEstado=1 and FORMAT(c.dFecha,'dd/MM/yyyy')=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy') 
	and dHoraSalida=''

	select  @nResultado=0

if @siExiste=0
  begin 
	insert into persona(cDocPer,cNombres,cApPaterno,cApMaterno,
	dFecReg,cUsuReg,dFecMod,cUsuMod) 
	values (@dni,@nombres,@apPaterno,@apMaterno,
	dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user) 

	SELECT @idPersona = @@IDENTITY  
  end
else
 begin
	select @idPersona=idPersona from persona where cDocPer=@dni and bEstado=1
 end

	if  @siUsadoPer=0
	begin
		insert into cargavehicular(nSucursal,dFecha,dHoraEntrada,dHoraSalida,
		idPersona, cLicencia, cEmpresaPvnte, idEmpresaDestino, cPlaca,
		cNumContenedor, bUnidad, bCarreta, cCodCarreta, cTipo,
		cEstadoCarga, cPrecintoGuia, 
		dFecReg, cUsuReg, dFecMod, cUsuMod) 
		values(@sucursal,@fecha,@horaEntrada,@horaSalida,
		@idPersona,@licencia,@empresa_pvnte,@iddestino,@placa,
		@numContenedor,@bUnidad,@bCarreta,@codCarreta,@tipo,
		@estado,@precinto,
		dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user) 

		select @nResultado=1
	end
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ins_cita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ins_cita]
@sucursal int=1,
@fecha varchar(10)='',
@hora varchar(10)='',
@dni varchar(10)='',
@nombres varchar(50)='',
@apPaterno varchar(50)='',
@apMaterno varchar(50)='',
@idDestino int=0,
@encargado varchar(200)='',
@empresapvnte varchar(200)='',
@observacion varchar(200)='',
@idcitador int,
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT

AS
 --SET NOCOUNT ON; --cuenta registros afectados cuando está en off
declare @idPersona int,
@siExiste int

	select @siExiste=count(*) from persona where cDocPer=@dni and bEstado=1
	select  @nResultado=0

BEGIN TRANSACTION
if @siExiste=0
  begin 
	insert into persona(cDocPer,cNombres,cApPaterno,cApMaterno,
	dFecReg,cUsuReg,dFecMod,cUsuMod) 
	values (@dni,@nombres,@apPaterno,@apMaterno,
	dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user) 

	SELECT @idPersona = @@IDENTITY  
  end
else
 begin
	select @idPersona=idPersona from persona where cDocPer=@dni and bEstado=1
 end

	
		insert into cita (nSucursal,dFecha,cHora,idPersona,
		idDestino,cEncargado,cEmpresaPvnte,cObservacion,idcitador,
		dFecReg,cUsuReg,dFecMod,cUsuMod) 
		values(@sucursal,@fecha,@hora,@idPersona,
		@idDestino,@encargado,@empresapvnte,@observacion,@idcitador,
		dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user) 

		select @nResultado=1
	

COMMIT TRANSACTION	
GO
/****** Object:  StoredProcedure [dbo].[sp_ins_citaAsistencia]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ins_citaAsistencia]
@sucursal int=1,
@idcita int=0,
@dni varchar(10)='',
@numAsignado varchar(10)='',
@destino varchar(200)='',
@encargado varchar(200)='',
@empresapvnte varchar(200)='',
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT

AS
 --SET NOCOUNT ON; --cuenta registros afectados cuando está en off
declare @idPersona int,
@siUsadoNum int
	
	select @idPersona=idpersona from persona where cDocPer=@dni and bEstado=1

	select @siUsadoNum=count(*) from movimientoPersona 
	where nNumAsignado=@numAsignado and ltrim(rtrim(nNumAsignado))<>'' 
	and bEstado=1 and FORMAT(dFecha,'dd/MM/yyyy')=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy') 
	and dHoraSalida=''

	select  @nResultado=0

if @siUsadoNum=0
begin  
BEGIN TRANSACTION
		
		update cita set bAtendido=1 where idcita=@idcita

		insert into movimientoPersona (nSucursal,idPersona,dFecha,dHoraEntrada,dHoraSalida,
		nNumAsignado,cDestino,cEncargado,cEmpresaPvnte,cObservacion,cModoMov,
		dFecReg,cUsuReg,dFecMod,cUsuMod) 
		values(@sucursal,@idPersona,FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy'),FORMAT(dbo.fn_get_fechaCorregida(@codPais),'HH:mm'),'',
		@numAsignado,@destino,@encargado,@empresapvnte,'CITA PROGRAMADA','c',
		dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user) 

		select @nResultado=1
	

COMMIT TRANSACTION
end
GO
/****** Object:  StoredProcedure [dbo].[sp_ins_empresa]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_ins_empresa]
@sucursal int,
@nombre varchar(50),
@ruc varchar(15),
@descripcion varchar(200),
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT
AS
 --SET NOCOUNT OFF;
 declare
  @exis_emp int;
  
BEGIN
	select @exis_emp=count(*) from empresa e where ltrim(rtrim(e.cRuc))=ltrim(rtrim(@ruc));
	select  @nResultado=0;
	if @exis_emp=0
	begin
			INSERT INTO empresa(nSucursal,cRuc,cNombre,cDescripcion,
			dFecReg,cUsuReg,dFecMod,cUsuMod) 
			values (@sucursal,@ruc,@nombre,@descripcion,
			dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user);
			select  @nResultado=1;
	end
 

END
GO
/****** Object:  StoredProcedure [dbo].[sp_ins_indeseado]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------
CREATE PROCEDURE [dbo].[sp_ins_indeseado]
@sucursal int=1,
@dni varchar(10)='',
@nombres varchar(50)='',
@apPaterno varchar(50)='',
@apMaterno varchar(50)='',
@cargo varchar(50)='',
@empresapvnte varchar(200)='',
@observacion varchar(200)='',
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT

AS
 --SET NOCOUNT ON; --cuenta registros afectados cuando está en off

declare @idPersona int,
@siExiste int,
@siUsadoPer int

	select @siExiste=count(*) from persona where cDocPer=@dni and bEstado=1
	select  @nResultado=0

BEGIN TRANSACTION
if @siExiste=0
  begin 
	insert into persona(cDocPer,cNombres,cApPaterno,cApMaterno,bIndeseado,
	dFecReg,cUsuReg,dFecMod,cUsuMod) 
	values (@dni,@nombres,@apPaterno,@apMaterno,1,
	dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user) 

	SELECT @idPersona = @@IDENTITY  
  end
else
 begin
	select @idPersona=idPersona from persona where cDocPer=@dni and bEstado=1
 end
	
	select @siUsadoPer=count(*) from indeseado where idPersona=@idPersona
	if  @siUsadoPer=0
	begin
		insert into indeseado (nSucursal,idPersona,cCargo,cEmpresaPvnte,
		cObservacion,dFecReg,cUsuReg,dFecMod,cUsuMod) 
		values(@sucursal,@idPersona,@cargo,@empresapvnte,
		@observacion,dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user) 

		select @nResultado=1
	end	
COMMIT TRANSACTION	

GO
/****** Object:  StoredProcedure [dbo].[sp_ins_movimiento]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_ins_movimiento]
@sucursal int=1,
@fecha varchar(10)='',
@horaEntrada varchar(10)='',
@horaSalida varchar(10)='',
@dni varchar(10)='',
@nombres varchar(50)='',
@apPaterno varchar(50)='',
@apMaterno varchar(50)='',
@numAsignado varchar(10)='',
@destino varchar(200)='',
@encargado varchar(200)='',
@empresapvnte varchar(200)='',
@observacion varchar(200)='',
@modo varchar(3)='v',
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT

AS
 --SET NOCOUNT ON; --cuenta registros afectados cuando está en off
declare @idPersona int,
@siExiste int,
@siUsadoNum int,
@siUsadoPer int

	select @siExiste=count(*) from persona where cDocPer=@dni and bEstado=1

	select @siUsadoNum=count(*) from movimientoPersona 
	where nNumAsignado=@numAsignado and ltrim(rtrim(nNumAsignado))<>'' 
	and bEstado=1 and FORMAT(dFecha,'dd/MM/yyyy')=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy') 
	and dHoraSalida=''

	select @siUsadoPer=count(*) from movimientoPersona m
	inner join persona p on p.idPersona =m.idPersona 
	where p.cDocPer=@dni and m.bEstado=1 and FORMAT(dFecha,'dd/MM/yyyy')=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy') 
	and dHoraSalida=''

	select  @nResultado=0

BEGIN TRANSACTION
if @siExiste=0
  begin 
	insert into persona(cDocPer,cNombres,cApPaterno,cApMaterno,
	dFecReg,cUsuReg,dFecMod,cUsuMod) 
	values (@dni,@nombres,@apPaterno,@apMaterno,
	dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user) 

	SELECT @idPersona = @@IDENTITY  
  end
else
 begin
	select @idPersona=idPersona from persona where cDocPer=@dni and bEstado=1
 end

	if @siUsadoNum=0  and @siUsadoPer=0
	begin
		insert into movimientoPersona (nSucursal,idPersona,dFecha,dHoraEntrada,dHoraSalida,
		nNumAsignado,cDestino,cEncargado,cEmpresaPvnte,cObservacion,cModoMov,
		dFecReg,cUsuReg,dFecMod,cUsuMod) 
		values(@sucursal,@idPersona,@fecha,@horaEntrada,@horaSalida,
		@numAsignado,@destino,@encargado,@empresapvnte,@observacion,@modo,
		dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user) 

		select @nResultado=1
	end


COMMIT TRANSACTION	
GO
/****** Object:  StoredProcedure [dbo].[Sp_segurobyid]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_segurobyid]
@idseguro int 
AS
 SET NOCOUNT ON; 
BEGIN 

select s.idseguro,s.nombre,s.nrosalud,s.nropoliza,FORMAT(s.fechainicio,'dd/MM/yyyy') fechainicio,
FORMAT(s.fechafin,'dd/MM/yyyy') fechafin,e.nombre empresa,e.idempresa 
from seguro s,empresa e 
where  s.idseguro=@idseguro and s.idempresa=e.idempresa ;

END



GO
/****** Object:  StoredProcedure [dbo].[Sp_trabajadorbyid]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sp_trabajadorbyid]
@idtrabajador int	
AS
SET NOCOUNT ON;
 
begin
	SELECT  t.idtrabajador,t.nombre, t.dni, t.cargo, t.area,t.habilitado,
	t.fecingreso,t.fecretiro, e.nombre  empresa,e.idempresa,
	(case 
		when (t.habilitado=1) then 'ACTIVO'
		else  'NO ACTIVO'
	end) estado,
	(case 
		when (t.habilitado=1) then 'GREEN'
		else  'RED'
	end) colorEstado,
	(case 
		when (t.habilitado=1) then 'checked'
		else  ''
	end) opcionsi,
	(case 
		when (t.habilitado=0) then ''
		else  ''
	end) opcionno   
	FROM trabajador t, empresa e
	where t.idempresa = e.idempresa  and t.idtrabajador=@idtrabajador;
end



GO
/****** Object:  StoredProcedure [dbo].[sp_upd_cargaVehicular]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_upd_cargaVehicular]
@idcarga int=0,
@licencia varchar(15)='',
@empresa_pvnte varchar(50)='',
@iddestino varchar(200)='',
@placa varchar(15)='',
@numContenedor varchar(15)='',
@bUnidad int=0,
@bCarreta int=0,
@codCarreta varchar(15)='',
@tipo varchar(5)='20',
@estado varchar(5)='V',
@precinto varchar(50)='',
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT

AS
 --SET NOCOUNT ON; --cuenta registros afectados cuando está en off
BEGIN 


	select  @nResultado=0

 
	begin
		update cargavehicular set cLicencia=@licencia,cEmpresaPvnte=@empresa_pvnte, 
		idEmpresaDestino=@iddestino, cPlaca=@placa,
		cNumContenedor=@numContenedor, bUnidad=@bUnidad, bCarreta=@bCarreta,
		 cCodCarreta=@codcarreta, cTipo=@tipo, 
		cEstadoCarga=@estado, cPrecintoGuia=@precinto, 
		 dFecMod=dbo.fn_get_fechaCorregida(@codPais), cUsuMod=@user
		 where idcargavehicular=@idcarga
		
		select @nResultado=1
	end
end
	
GO
/****** Object:  StoredProcedure [dbo].[sp_upd_cargaVehicularSalida]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_upd_cargaVehicularSalida]
@idcarga int,
@codPais varchar(5)='PER',
@user varchar(50)=''
AS
 SET NOCOUNT OFF; --cuenta registros afectados cuando está en off
BEGIN 

	update cargaVehicular set dHoraSalida=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'HH:mm'),
	dFecMod=dbo.fn_get_fechaCorregida(@codPais),
	cUsuMod=@user WHERE idcargavehicular=@idcarga
	
END
	
GO
/****** Object:  StoredProcedure [dbo].[sp_upd_cita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_upd_cita]
@idcita int=0,
@fecha varchar(10)='',
@hora varchar(10)='',
@dni varchar(10)='',
@nombres varchar(50)='',
@apPaterno varchar(50)='',
@apMaterno varchar(50)='',
@idDestino varchar(200)='',
@encargado varchar(200)='',
@empresapvnte varchar(200)='',
@observacion varchar(200)='',
@idcitador int,
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT

AS
 --SET NOCOUNT ON; --cuenta registros afectados cuando está en off
declare @idPersona int,
@siExiste int


	select @siExiste=count(*) from persona where cDocPer=@dni and bEstado=1
	select  @nResultado=0

BEGIN TRANSACTION
if @siExiste=0
  begin 
	insert into persona(cDocPer,cNombres,cApPaterno,cApMaterno,
	dFecReg,cUsuReg,dFecMod,cUsuMod) 
	values (@dni,@nombres,@apPaterno,@apMaterno,
	dbo.fn_get_fechaCorregida(@codPais),@user,dbo.fn_get_fechaCorregida(@codPais),@user) 

	SELECT @idPersona = @@IDENTITY  
  end
else
 begin
	select @idPersona=idPersona from persona where cDocPer=@dni and bEstado=1
 end

		update cita set dFecha=@fecha,cHora=@hora,idPersona=@idPersona,
		idDestino=@idDestino,cEncargado=@encargado,cEmpresaPvnte=@empresapvnte,
		cObservacion=@observacion,dFecMod=dbo.fn_get_fechaCorregida(@codPais),
		cUsuMod=@user where idcita=@idcita
		
		select @nResultado=1
	
COMMIT TRANSACTION	
GO
/****** Object:  StoredProcedure [dbo].[sp_upd_empresa]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_upd_empresa]
@idempresa int,
@nombre varchar(100),
@descripcion varchar(200)
AS
 SET NOCOUNT OFF; 
BEGIN 
	
update empresa 
set nombre=@nombre,descripcion=@descripcion,
usumodificacion='MACS',fecmodificacion=dateadd(hour,1,getdate())
 where idempresa=@idempresa;
END

GO
/****** Object:  StoredProcedure [dbo].[sp_upd_indeseado]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_upd_indeseado]
@idindeseado int ,
@cargo varchar(50)='',
@empresapvnte varchar(200)='',
@observacion varchar(200)='',
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT

AS
 --SET NOCOUNT ON; --cuenta registros afectados cuando está en off
BEGIN TRANSACTION
		update indeseado set cCargo=@cargo,
		 cEmpresaPvnte=@empresapvnte,cObservacion=@observacion,
		 dFecMod=dbo.fn_get_fechaCorregida(@codPais),
		cUsuMod=@user where idindeseado=@idindeseado
		
		select @nResultado=1
			
COMMIT TRANSACTION	
GO
/****** Object:  StoredProcedure [dbo].[sp_upd_movimiento]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_upd_movimiento]
@idmovimiento int ,
@numAsignado varchar(10)='',
@destino varchar(200)='',
@encargado varchar(200)='',
@empresapvnte varchar(200)='',
@observacion varchar(200)='',
@codPais varchar(5)='PER',
@user varchar(50)='',
@nResultado int OUTPUT

AS
 --SET NOCOUNT ON; --cuenta registros afectados cuando está en off
BEGIN 
declare @idPersona int,
@siUsadoNum int

	select @idPersona=idPersona  from movimientoPersona m
	where M.idMovimiento=@idmovimiento 

	select @siUsadoNum=count(*) from movimientoPersona 
	where nNumAsignado=@numAsignado and ltrim(rtrim(nNumAsignado))<>''
	and bEstado=1 and FORMAT(dFecha,'dd/MM/yyyy')=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'dd/MM/yyyy') 
	and dHoraSalida='' and idPersona<>@idPersona

	select  @nResultado=0

	if @siUsadoNum=0  
	BEGIN TRANSACTION

		update movimientoPersona set nNumAsignado=@numAsignado,
		cDestino=@destino, cEncargado=@encargado, cEmpresaPvnte=@empresapvnte,
		cObservacion=@observacion, dFecMod=dbo.fn_get_fechaCorregida(@codPais),
		cUsuMod=@user where idMovimiento=@idmovimiento
		
		select @nResultado=1
	COMMIT TRANSACTION
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_upd_movimientoSalida]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_upd_movimientoSalida]
@idmovimiento int,
@codPais varchar(5)='PER',
@user varchar(50)=''
AS
 SET NOCOUNT OFF; --cuenta registros afectados cuando está en off
BEGIN TRANSACTION

	update movimientoPersona set dHoraSalida=FORMAT(dbo.fn_get_fechaCorregida(@codPais),'HH:mm'),
	dFecMod=dbo.fn_get_fechaCorregida(@codPais),
	cUsuMod=@user WHERE idMovimiento=@idmovimiento
		
COMMIT TRANSACTION


GO
/****** Object:  StoredProcedure [dbo].[Sp_updatseguro]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_updatseguro]
@idseguro int,
@nombre varchar(50),
@nrosalud varchar(20),
@nropoliza varchar(20),
@fechainicio varchar(10),
@fechafin varchar(10)
AS
 SET NOCOUNT OFF; 
BEGIN 

set @fechainicio=CONCAT(SUBSTRING(@fechainicio, 7, 4), '/',SUBSTRING(@fechainicio, 4, 2) , '/', SUBSTRING(@fechainicio, 1, 2));
set @fechafin=CONCAT(SUBSTRING(@fechafin, 7, 4), '/',SUBSTRING(@fechafin, 4, 2) , '/', SUBSTRING(@fechafin, 1, 2));
if @fechainicio<=@fechafin
	begin
		update seguro set nombre=@nombre,nrosalud=@nrosalud,
		nropoliza=@nropoliza,fechainicio=@fechainicio,fechafin=@fechafin,
		usumodificacion='MACS',fecmodificacion=dateadd(hour,1,getdate()) 
		where idseguro=@idseguro ;
	end

END

GO
/****** Object:  UserDefinedFunction [dbo].[fn_get_fechaCorregida]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Scalar Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 



CREATE FUNCTION [dbo].[fn_get_fechaCorregida] 
(
	@codPais varchar(3)
)
RETURNS  datetime
AS
BEGIN

Declare @fecha datetime
	
	if @codPais='PER'
	BEGIN
		set @fecha=DATEADD(HOUR,0,GETDATE())
	END 

	
	-- Return the result of the function
	RETURN @fecha

END

GO
/****** Object:  UserDefinedFunction [dbo].[fn_get_verificarIndeseado]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_get_verificarIndeseado]
(
	@dni varchar(10)
)
RETURNS  bit
AS
BEGIN

	Declare @isIndeseado bit=0,
		@siExiste int
	
		select @siExiste=count(*) from indeseado i
		inner join persona p on p.idpersona=i.idpersona
		where p.cDocPer=@dni and i.bEstado=1 and p.bEstado=1
		
		if @siExiste>0
		BEGIN
			set @isIndeseado=1
		END 

	RETURN @isIndeseado

END



GO
/****** Object:  Table [dbo].[area]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[area](
	[idArea] [int] IDENTITY(1,1) NOT NULL,
	[cNombre] [varchar](50) NOT NULL,
	[cDescripcion] [varchar](100) NULL,
	[bEstado] [int] NOT NULL,
	[dFecReg] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[asistencia]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[asistencia](
	[idasistencia] [int] IDENTITY(1,1) NOT NULL,
	[fecha] [date] NOT NULL,
	[horainicio] [varchar](20) NOT NULL,
	[horasalida] [varchar](20) NULL,
	[idtrabajador] [int] NOT NULL,
 CONSTRAINT [PK_asistencia] PRIMARY KEY CLUSTERED 
(
	[idasistencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cargaVehicular]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cargaVehicular](
	[idCargaVehicular] [int] IDENTITY(1,1) NOT NULL,
	[dFecha] [datetime] NOT NULL,
	[dHoraEntrada] [varchar](10) NOT NULL,
	[dHoraSalida] [varchar](10) NOT NULL,
	[idPersona] [int] NOT NULL,
	[cLicencia] [varchar](15) NOT NULL,
	[cEmpresaPvnte] [varchar](50) NOT NULL,
	[idEmpresaDestino] [int] NOT NULL,
	[cPlaca] [varchar](15) NOT NULL,
	[cNumContenedor] [varchar](15) NOT NULL,
	[bUnidad] [int] NOT NULL,
	[bCarreta] [int] NOT NULL,
	[cCodCarreta] [varchar](15) NULL,
	[cTipo] [varchar](5) NOT NULL,
	[cEstadoCarga] [varchar](5) NOT NULL,
	[cPrecintoGuia] [varchar](50) NULL,
	[bEstado] [int] NOT NULL,
	[dFecReg] [datetime] NOT NULL,
	[cUsuReg] [varchar](50) NOT NULL,
	[dFecMod] [datetime] NOT NULL,
	[cUsuMod] [varchar](50) NOT NULL,
	[nSucursal] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cita]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cita](
	[idcita] [int] IDENTITY(1,1) NOT NULL,
	[dfecha] [datetime] NOT NULL,
	[cHora] [varchar](10) NOT NULL,
	[idPersona] [int] NOT NULL,
	[cEncargado] [varchar](200) NOT NULL,
	[cEmpresaPvnte] [varchar](200) NOT NULL,
	[cObservacion] [varchar](200) NOT NULL,
	[idCitador] [int] NOT NULL,
	[bOculto] [int] NOT NULL,
	[bEstado] [int] NOT NULL,
	[dFecReg] [datetime] NOT NULL,
	[cUsuReg] [varchar](50) NOT NULL,
	[dFecMod] [datetime] NOT NULL,
	[cUsuMod] [varchar](50) NOT NULL,
	[nSucursal] [int] NOT NULL,
	[bAtendido] [int] NOT NULL,
	[idDestino] [int] NOT NULL,
 CONSTRAINT [PK_citad] PRIMARY KEY CLUSTERED 
(
	[idcita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[control]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[control](
	[idcontrol] [int] IDENTITY(1,1) NOT NULL,
	[fechaentrada] [date] NOT NULL,
	[horaentrada] [varchar](10) NOT NULL,
	[fechasalida] [date] NOT NULL,
	[horasalida] [varchar](10) NULL,
	[idtrabajador] [int] NOT NULL,
	[turno] [varchar](8) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[empresa]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[empresa](
	[idEmpresa] [int] IDENTITY(1,1) NOT NULL,
	[nSucursal] [int] NOT NULL,
	[cRuc] [varchar](15) NOT NULL,
	[cNombre] [varchar](50) NOT NULL,
	[cDescripcion] [varchar](200) NULL,
	[bEstado] [int] NOT NULL,
	[dFecReg] [datetime] NOT NULL,
	[cUsuReg] [varchar](50) NOT NULL,
	[dFecMod] [datetime] NOT NULL,
	[cUsuMod] [varchar](50) NOT NULL,
 CONSTRAINT [PK_empres] PRIMARY KEY CLUSTERED 
(
	[idEmpresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[indeseado]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[indeseado](
	[idIndeseado] [int] IDENTITY(1,1) NOT NULL,
	[nSucursal] [int] NOT NULL,
	[idPersona] [int] NOT NULL,
	[cCargo] [varchar](50) NOT NULL,
	[cEmpresaPvnte] [varchar](50) NOT NULL,
	[cObservacion] [varchar](200) NOT NULL,
	[bEstado] [int] NOT NULL,
	[dFecReg] [datetime] NOT NULL,
	[cUsuReg] [varchar](50) NOT NULL,
	[dFecMod] [datetime] NOT NULL,
	[cUsuMod] [varchar](50) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[menuSistema]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[menuSistema](
	[idMenu] [int] IDENTITY(1,1) NOT NULL,
	[cCodMenu] [varchar](5) NOT NULL,
	[cCodOpcion] [varchar](5) NOT NULL,
	[cDescripcion] [varchar](200) NOT NULL,
	[bEstado] [int] NOT NULL,
	[dFecReg] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[movimientoPersona]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[movimientoPersona](
	[idMovimiento] [int] IDENTITY(1,1) NOT NULL,
	[nSucursal] [int] NOT NULL,
	[idPersona] [int] NOT NULL,
	[dFecha] [datetime] NOT NULL,
	[dHoraEntrada] [varchar](10) NOT NULL,
	[dHoraSalida] [varchar](10) NOT NULL,
	[nNumAsignado] [varchar](10) NOT NULL,
	[cDestino] [varchar](200) NOT NULL,
	[cEncargado] [varchar](200) NOT NULL,
	[cEmpresaPvnte] [varchar](200) NOT NULL,
	[cObservacion] [varchar](200) NOT NULL,
	[cModoMov] [varchar](3) NOT NULL,
	[bEstado] [int] NOT NULL,
	[dFecReg] [datetime] NOT NULL,
	[cUsuReg] [varchar](50) NOT NULL,
	[dFecMod] [datetime] NOT NULL,
	[cUsuMod] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MovimientoPersona] PRIMARY KEY CLUSTERED 
(
	[idMovimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[persona]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[persona](
	[idPersona] [int] IDENTITY(1,1) NOT NULL,
	[cDocPer] [varchar](10) NOT NULL,
	[cNombres] [varchar](50) NOT NULL,
	[cApPaterno] [varchar](50) NOT NULL,
	[cApMaterno] [varchar](50) NOT NULL,
	[bTrabajador] [int] NOT NULL,
	[bIndeseado] [int] NOT NULL,
	[bEstado] [int] NOT NULL,
	[dFecReg] [datetime] NOT NULL,
	[cUsuReg] [varchar](50) NOT NULL,
	[dFecMod] [datetime] NOT NULL,
	[cUsuMod] [varchar](50) NOT NULL,
 CONSTRAINT [PK_persona] PRIMARY KEY CLUSTERED 
(
	[idPersona] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[prueba]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[prueba](
	[idPrueba] [int] IDENTITY(1,1) NOT NULL,
	[dFecha] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[seguro]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[seguro](
	[idseguro] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[nrosalud] [varchar](20) NOT NULL,
	[nropoliza] [varchar](20) NOT NULL,
	[fechainicio] [date] NOT NULL,
	[fechafin] [date] NOT NULL,
	[idempresa] [int] NOT NULL,
	[flagactivo] [int] NOT NULL,
	[usucreacion] [varchar](20) NULL,
	[feccreacion] [datetime] NULL,
	[usumodificacion] [varchar](20) NULL,
	[fecmodificacion] [datetime] NULL,
 CONSTRAINT [PK_seguro] PRIMARY KEY CLUSTERED 
(
	[idseguro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[sucursal]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sucursal](
	[idSucursal] [int] IDENTITY(1,1) NOT NULL,
	[cNombre] [varchar](50) NULL,
	[bEstado] [int] NULL,
	[dFecReg] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[trabajador]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[trabajador](
	[idtrabajador] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[dni] [varchar](10) NOT NULL,
	[cargo] [varchar](100) NULL,
	[area] [varchar](100) NULL,
	[habilitado] [int] NOT NULL,
	[idempresa] [int] NOT NULL,
	[usuregistro] [varchar](20) NULL,
	[fecregistro] [datetime] NULL,
	[usumodificacion] [varchar](20) NULL,
	[fecmodificacion] [datetime] NULL,
	[fecretiro] [date] NULL,
	[fecingreso] [date] NULL,
 CONSTRAINT [PK_trabajador] PRIMARY KEY CLUSTERED 
(
	[idtrabajador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[trabajadorcuenta]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[trabajadorcuenta](
	[idtrabajador] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[apellidoPaterno] [varchar](50) NOT NULL,
	[apellidoMaterno] [varchar](50) NOT NULL,
	[dni] [varchar](8) NOT NULL,
	[cargo] [varchar](50) NOT NULL,
	[identificador] [varchar](50) NOT NULL,
	[tipocuenta] [varchar](100) NOT NULL,
	[activo] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[usuario]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[usuario](
	[idUsuario] [int] IDENTITY(1,1) NOT NULL,
	[nSucursal] [int] NOT NULL,
	[cUsuNombre] [varchar](50) NOT NULL,
	[cUsuCtseña] [varchar](50) NOT NULL,
	[idTrabajador] [int] NOT NULL,
	[idArea] [int] NOT NULL,
	[bActivo] [int] NOT NULL,
	[dFecReg] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[usuariosAccesos]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usuariosAccesos](
	[idAcceso] [int] IDENTITY(1,1) NOT NULL,
	[idMenu] [int] NOT NULL,
	[idUsuario] [int] NOT NULL,
	[bAbrir] [int] NOT NULL,
	[bGuardar] [int] NOT NULL,
	[bCerrar] [int] NOT NULL,
	[bExportar] [int] NOT NULL,
	[bEliminar] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[visitante]    Script Date: 08/02/2022 09:40:55 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[visitante](
	[idvisitante] [int] IDENTITY(1,1) NOT NULL,
	[fecha] [date] NOT NULL,
	[horaentrada] [varchar](50) NOT NULL,
	[horasalida] [varchar](50) NULL,
	[nombre] [varchar](50) NOT NULL,
	[dni] [int] NOT NULL,
	[destino] [varchar](50) NOT NULL,
	[encargado] [varchar](50) NOT NULL,
	[empresa_proveniente] [varchar](100) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[area] ON 

INSERT [dbo].[area] ([idArea], [cNombre], [cDescripcion], [bEstado], [dFecReg]) VALUES (1, N'SISTEMAS', N'', 1, CAST(0x0000AC4B016E23FA AS DateTime))
INSERT [dbo].[area] ([idArea], [cNombre], [cDescripcion], [bEstado], [dFecReg]) VALUES (2, N'SEGURIDAD', N'', 1, CAST(0x0000AC4B016E345C AS DateTime))
SET IDENTITY_INSERT [dbo].[area] OFF
SET IDENTITY_INSERT [dbo].[cargaVehicular] ON 

INSERT [dbo].[cargaVehicular] ([idCargaVehicular], [dFecha], [dHoraEntrada], [dHoraSalida], [idPersona], [cLicencia], [cEmpresaPvnte], [idEmpresaDestino], [cPlaca], [cNumContenedor], [bUnidad], [bCarreta], [cCodCarreta], [cTipo], [cEstadoCarga], [cPrecintoGuia], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod], [nSucursal]) VALUES (1, CAST(0x0000AC6500000000 AS DateTime), N'13:42', N'', 1, N'23234343', N'-', 1, N'DFD3433', N'', 0, 0, N' ', N'20', N'V', N'', 1, CAST(0x0000AC6500E2110F AS DateTime), N'ADMIN', CAST(0x0000AC6500E2110F AS DateTime), N'ADMIN', 1)
INSERT [dbo].[cargaVehicular] ([idCargaVehicular], [dFecha], [dHoraEntrada], [dHoraSalida], [idPersona], [cLicencia], [cEmpresaPvnte], [idEmpresaDestino], [cPlaca], [cNumContenedor], [bUnidad], [bCarreta], [cCodCarreta], [cTipo], [cEstadoCarga], [cPrecintoGuia], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod], [nSucursal]) VALUES (2, CAST(0x0000AC6600000000 AS DateTime), N'12:08', N'20:55', 1, N'23234343', N'-', 4, N'FFGF434', N'', 1, 0, N' ', N'40', N'LL', N'', 1, CAST(0x0000AC6600C89400 AS DateTime), N'ADMIN', CAST(0x0000AC660158EB50 AS DateTime), N'ADMIN', 1)
INSERT [dbo].[cargaVehicular] ([idCargaVehicular], [dFecha], [dHoraEntrada], [dHoraSalida], [idPersona], [cLicencia], [cEmpresaPvnte], [idEmpresaDestino], [cPlaca], [cNumContenedor], [bUnidad], [bCarreta], [cCodCarreta], [cTipo], [cEstadoCarga], [cPrecintoGuia], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod], [nSucursal]) VALUES (3, CAST(0x0000AC6600000000 AS DateTime), N'12:12', N'', 1, N'23234343', N'-', 1, N'FFGF434', N'', 0, 0, N' ', N'20', N'V', N'', 1, CAST(0x0000AC6600C9255A AS DateTime), N'ADMIN', CAST(0x0000AC660158D3DF AS DateTime), N'ADMIN', 1)
INSERT [dbo].[cargaVehicular] ([idCargaVehicular], [dFecha], [dHoraEntrada], [dHoraSalida], [idPersona], [cLicencia], [cEmpresaPvnte], [idEmpresaDestino], [cPlaca], [cNumContenedor], [bUnidad], [bCarreta], [cCodCarreta], [cTipo], [cEstadoCarga], [cPrecintoGuia], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod], [nSucursal]) VALUES (4, CAST(0x0000AC6600000000 AS DateTime), N'13:19', N'', 4, N'23234343', N'-', 4, N'DFD3434', N'', 0, 1, N'0001', N'40', N'V', N'', 1, CAST(0x0000AC6600DBC79B AS DateTime), N'ADMIN', CAST(0x0000AC6600E05414 AS DateTime), N'ADMIN', 1)
INSERT [dbo].[cargaVehicular] ([idCargaVehicular], [dFecha], [dHoraEntrada], [dHoraSalida], [idPersona], [cLicencia], [cEmpresaPvnte], [idEmpresaDestino], [cPlaca], [cNumContenedor], [bUnidad], [bCarreta], [cCodCarreta], [cTipo], [cEstadoCarga], [cPrecintoGuia], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod], [nSucursal]) VALUES (5, CAST(0x0000AC6800000000 AS DateTime), N'18:30', N'18:37', 7, N'34354345', N'-', 1, N'FFGF434', N'', 1, 0, N' ', N'20', N'LL', N'NINGUNO', 1, CAST(0x0000AC680131916D AS DateTime), N'ADMIN', CAST(0x0000AC680132E6DC AS DateTime), N'ADMIN', 1)
SET IDENTITY_INSERT [dbo].[cargaVehicular] OFF
SET IDENTITY_INSERT [dbo].[cita] ON 

INSERT [dbo].[cita] ([idcita], [dfecha], [cHora], [idPersona], [cEncargado], [cEmpresaPvnte], [cObservacion], [idCitador], [bOculto], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod], [nSucursal], [bAtendido], [idDestino]) VALUES (1, CAST(0x0000ACD500000000 AS DateTime), N'22:20', 1, N'Keny Huamani', N'-', N'ENTREVISTA DE TRABAJO PENDIENTE', 1, 0, 1, CAST(0x0000ACE8016D9FAC AS DateTime), N'ADMIN', CAST(0x0000ACEF0175A6B8 AS DateTime), N'ADMIN', 1, 1, 4)
INSERT [dbo].[cita] ([idcita], [dfecha], [cHora], [idPersona], [cEncargado], [cEmpresaPvnte], [cObservacion], [idCitador], [bOculto], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod], [nSucursal], [bAtendido], [idDestino]) VALUES (2, CAST(0x0000ACF000000000 AS DateTime), N'16:00', 4, N'Keny Huamani', N'-', N'-', 1, 0, 1, CAST(0x0000ACED01586125 AS DateTime), N'ADMIN', CAST(0x0000ACEF0175B2B2 AS DateTime), N'ADMIN', 1, 1, 1)
INSERT [dbo].[cita] ([idcita], [dfecha], [cHora], [idPersona], [cEncargado], [cEmpresaPvnte], [cObservacion], [idCitador], [bOculto], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod], [nSucursal], [bAtendido], [idDestino]) VALUES (3, CAST(0x0000ACF000000000 AS DateTime), N'14:53', 7, N'Keny Huamani', N'-', N'ENTREVISTA DE TRABAJO', 1, 0, 1, CAST(0x0000ACF000F58BB0 AS DateTime), N'ADMIN', CAST(0x0000ACF000F58BB0 AS DateTime), N'ADMIN', 1, 0, 4)
INSERT [dbo].[cita] ([idcita], [dfecha], [cHora], [idPersona], [cEncargado], [cEmpresaPvnte], [cObservacion], [idCitador], [bOculto], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod], [nSucursal], [bAtendido], [idDestino]) VALUES (4, CAST(0x0000AE3600000000 AS DateTime), N'14:30', 4, N'-', N'-', N'ENTREVISTA DE TRABAJO', 1, 0, 1, CAST(0x0000AE3600ED7763 AS DateTime), N'ADMIN', CAST(0x0000AE3600ED7763 AS DateTime), N'ADMIN', 1, 0, 4)
SET IDENTITY_INSERT [dbo].[cita] OFF
SET IDENTITY_INSERT [dbo].[empresa] ON 

INSERT [dbo].[empresa] ([idEmpresa], [nSucursal], [cRuc], [cNombre], [cDescripcion], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (1, 1, N'20503423287', N'CENTELSA PERU S.A.C', N'', 1, CAST(0x0000AC5F00F367D3 AS DateTime), N'ADMIN', CAST(0x0000AC5F00F367D3 AS DateTime), N'ADMIN')
INSERT [dbo].[empresa] ([idEmpresa], [nSucursal], [cRuc], [cNombre], [cDescripcion], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (2, 1, N'20422626175', N'MICHELIN PERU S.A.', N'', 1, CAST(0x0000AC5F00F437F1 AS DateTime), N'ADMIN', CAST(0x0000AC5F00F437F1 AS DateTime), N'ADMIN')
INSERT [dbo].[empresa] ([idEmpresa], [nSucursal], [cRuc], [cNombre], [cDescripcion], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (3, 1, N'20110696494', N'DAEWOO PERU S.A', N'', 1, CAST(0x0000AC5F00F4A557 AS DateTime), N'ADMIN', CAST(0x0000AC5F00F4A557 AS DateTime), N'ADMIN')
INSERT [dbo].[empresa] ([idEmpresa], [nSucursal], [cRuc], [cNombre], [cDescripcion], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (4, 1, N'20333972248', N'IMUDESA', N'Inversiones Maritimas Universales Depositos S.A.', 1, CAST(0x0000AC5F00F51721 AS DateTime), N'ADMIN', CAST(0x0000AC5F00F51721 AS DateTime), N'ADMIN')
SET IDENTITY_INSERT [dbo].[empresa] OFF
SET IDENTITY_INSERT [dbo].[indeseado] ON 

INSERT [dbo].[indeseado] ([idIndeseado], [nSucursal], [idPersona], [cCargo], [cEmpresaPvnte], [cObservacion], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (1, 1, 5, N'', N'CONSTRUCCIONES UNION SA', N'SUPLANTAR IDENTIDAD DE TRABAJADOR DE LA EMPRESA', 1, CAST(0x0000AC5D0180C723 AS DateTime), N'ADMIN', CAST(0x0000ACDB017EDF4A AS DateTime), N'ADMIN')
INSERT [dbo].[indeseado] ([idIndeseado], [nSucursal], [idPersona], [cCargo], [cEmpresaPvnte], [cObservacion], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (2, 1, 6, N'CUADRILLERO', N'JUESMMAN', N'ANTECEDENTES PENALES.', 1, CAST(0x0000AC5D0184A70D AS DateTime), N'ADMIN', CAST(0x0000AC5E00816FB8 AS DateTime), N'ADMIN')
INSERT [dbo].[indeseado] ([idIndeseado], [nSucursal], [idPersona], [cCargo], [cEmpresaPvnte], [cObservacion], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (3, 1, 8, N'AUXILIAR', N'CENTELSA', N'FALTO EL RESPETO LOS AVPS', 1, CAST(0x0000ACDB017726CA AS DateTime), N'ADMIN', CAST(0x0000ACDB017726CA AS DateTime), N'ADMIN')
SET IDENTITY_INSERT [dbo].[indeseado] OFF
SET IDENTITY_INSERT [dbo].[movimientoPersona] ON 

INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (1, 1, 1, CAST(0x0000ACEF00000000 AS DateTime), N'22:37', N'', N'2', N'CASA', N'-', N'-', N'-', N'v', 1, CAST(0x0000AC520174FEFE AS DateTime), N'ADMIN', CAST(0x0000AC520174FEFE AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (2, 1, 1, CAST(0x0000ACEF00000000 AS DateTime), N'20:34', N'23:01', N'2', N'CASA', N'', N'', N'', N'v', 1, CAST(0x0000AC5401543FAF AS DateTime), N'ADMIN', CAST(0x0000AC54017B8A65 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (3, 1, 2, CAST(0x0000ACEF00000000 AS DateTime), N'20:48', N'', N'1', N'CASA', N'', N'', N'', N'v', 1, CAST(0x0000AC540157A90A AS DateTime), N'ADMIN', CAST(0x0000AC540157A90A AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (4, 1, 3, CAST(0x0000ACEF00000000 AS DateTime), N'22:11', N'', N'4', N'', N'', N'', N'', N'v', 1, CAST(0x0000AC54016DE7D9 AS DateTime), N'ADMIN', CAST(0x0000AC54016DE7D9 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (5, 1, 3, CAST(0x0000ACEF00000000 AS DateTime), N'00:14', N'21:24', N'2', N'CASA', N'-', N'-', N'', N'v', 1, CAST(0x0000AC5600041AB5 AS DateTime), N'ADMIN', CAST(0x0000AC560160DB9B AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (6, 1, 4, CAST(0x0000ACEF00000000 AS DateTime), N'21:20', N'', N'4', N'', N'', N'', N'', N'v', 1, CAST(0x0000AC5601604A2B AS DateTime), N'ADMIN', CAST(0x0000AC560160F4A2 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (7, 1, 1, CAST(0x0000ACEF00000000 AS DateTime), N'21:14', N'21:21', N'2', N'', N'', N'', N'', N'v', 1, CAST(0x0000AC5B015E56E3 AS DateTime), N'ADMIN', CAST(0x0000AC5B016013EE AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (8, 1, 2, CAST(0x0000ACEF00000000 AS DateTime), N'21:22', N'', N'3', N'CASA', N'', N'', N'', N'v', 1, CAST(0x0000AC5B0160451B AS DateTime), N'ADMIN', CAST(0x0000AC5B017EE990 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (9, 1, 1, CAST(0x0000ACEF00000000 AS DateTime), N'00:16', N'21:06', N'', N'', N'', N'', N'', N'v', 1, CAST(0x0000AC5E000499DB AS DateTime), N'ADMIN', CAST(0x0000AC5E015BC7E5 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (10, 1, 2, CAST(0x0000ACEF00000000 AS DateTime), N'08:58', N'', N'', N'', N'', N'', N'', N'v', 1, CAST(0x0000AC5E009419C9 AS DateTime), N'ADMIN', CAST(0x0000AC5E015C6A49 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (11, 1, 5, CAST(0x0000ACEF00000000 AS DateTime), N'20:27', N'', N'4', N'', N'', N'', N'', N'i', 1, CAST(0x0000AC5E01513AFC AS DateTime), N'ADMIN', CAST(0x0000AC5E01513AFC AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (12, 1, 4, CAST(0x0000ACEF00000000 AS DateTime), N'20:28', N'', N'7', N'', N'', N'', N'', N'v', 1, CAST(0x0000AC5E01517F1A AS DateTime), N'ADMIN', CAST(0x0000AC5E01517F1A AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (13, 1, 3, CAST(0x0000ACEF00000000 AS DateTime), N'20:31', N'21:07', N'1', N'', N'', N'', N'', N'v', 1, CAST(0x0000AC5E01535014 AS DateTime), N'ADMIN', CAST(0x0000AC5E015C1015 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (14, 1, 6, CAST(0x0000ACEF00000000 AS DateTime), N'20:51', N'', N'', N'', N'', N'', N'', N'i', 1, CAST(0x0000AC5E015B2698 AS DateTime), N'ADMIN', CAST(0x0000AC5E015B2698 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (15, 1, 1, CAST(0x0000ACEF00000000 AS DateTime), N'21:06', N'', N'5', N'', N'', N'', N'', N'v', 1, CAST(0x0000AC5E015BDA10 AS DateTime), N'ADMIN', CAST(0x0000AC5E015C8B6E AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (16, 1, 3, CAST(0x0000ACEF00000000 AS DateTime), N'21:07', N'10:47', N'', N'', N'', N'', N'', N'v', 1, CAST(0x0000AC5E015C2F31 AS DateTime), N'ADMIN', CAST(0x0000ACF000B1B889 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (17, 1, 5, CAST(0x0000ACEF00000000 AS DateTime), N'00:00', N'', N'', N'', N'', N'-', N'', N'i', 1, CAST(0x0000AC68012F140F AS DateTime), N'ADMIN', CAST(0x0000AC68012F140F AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (18, 1, 1, CAST(0x0000ACEF00000000 AS DateTime), N'18:57', N'', N'2', N'CENTELSA', N'-', N'-', N'', N'v', 1, CAST(0x0000ACC6013872A0 AS DateTime), N'ADMIN', CAST(0x0000ACC6013872A0 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (19, 1, 1, CAST(0x0000ACEF00000000 AS DateTime), N'19:05', N'19:10', N'2', N'CENTELSA', N'-', N'-', N'', N'v', 1, CAST(0x0000ACD1013B9100 AS DateTime), N'ADMIN', CAST(0x0000ACD1013C07FC AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (20, 1, 1, CAST(0x0000ACEF00000000 AS DateTime), N'23:29', N'', N'2', N'CENTELSA', N'', N'-', N'', N'v', 1, CAST(0x0000ACD701835691 AS DateTime), N'ADMIN', CAST(0x0000ACD701835691 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (21, 1, 1, CAST(0x0000ACEF00000000 AS DateTime), N'12:23', N'15:59', N'21', N'CASA', N'-', N'-', N'-', N'v', 1, CAST(0x0000ACDA00CC87EC AS DateTime), N'ADMIN', CAST(0x0000ACDA01077505 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (22, 1, 4, CAST(0x0000ACF000000000 AS DateTime), N'07:55', N'11:20', N'21', N'CENTELSA', N'', N'', N'', N'v', 1, CAST(0x0000ACDB0082BDAC AS DateTime), N'ADMIN', CAST(0x0000ACF000BAD4D7 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (23, 1, 8, CAST(0x0000ACF000000000 AS DateTime), N'23:54', N'', N'2', N'CASA', N'', N'', N'', N'i', 1, CAST(0x0000ACDB018A19EF AS DateTime), N'ADMIN', CAST(0x0000ACDB018A19EF AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (28, 1, 1, CAST(0x0000ACF000000000 AS DateTime), N'11:37', N'11:43', N'15', N'IMUDESA', N'Keny Huamani', N'-', N'CITA PROGRAMADA', N'c', 1, CAST(0x0000ACF000BFAC9A AS DateTime), N'ADMIN', CAST(0x0000ACF000C14FED AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (29, 1, 4, CAST(0x0000ACF000000000 AS DateTime), N'11:42', N'', N'3', N'CENTELSA PERU S.A.C', N'Keny Huamani', N'-', N'CITA PROGRAMADA', N'c', 1, CAST(0x0000ACF000C0F409 AS DateTime), N'ADMIN', CAST(0x0000ACF000C0F409 AS DateTime), N'ADMIN')
INSERT [dbo].[movimientoPersona] ([idMovimiento], [nSucursal], [idPersona], [dFecha], [dHoraEntrada], [dHoraSalida], [nNumAsignado], [cDestino], [cEncargado], [cEmpresaPvnte], [cObservacion], [cModoMov], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (30, 1, 1, CAST(0x0000AE3600000000 AS DateTime), N'14:06', N'', N'0', N'CENTELSA', N'-', N'-', N'', N'v', 1, CAST(0x0000AE3600EA70D9 AS DateTime), N'ADMIN', CAST(0x0000AE3600EB36F1 AS DateTime), N'ADMIN')
SET IDENTITY_INSERT [dbo].[movimientoPersona] OFF
SET IDENTITY_INSERT [dbo].[persona] ON 

INSERT [dbo].[persona] ([idPersona], [cDocPer], [cNombres], [cApPaterno], [cApMaterno], [bTrabajador], [bIndeseado], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (1, N'47325183', N'MARCO', N'CUEVA', N'SÁNCHEZ', 0, 0, 1, CAST(0x0000AC520174FEFE AS DateTime), N'ADMIN', CAST(0x0000AC520174FEFE AS DateTime), N'ADMIN')
INSERT [dbo].[persona] ([idPersona], [cDocPer], [cNombres], [cApPaterno], [cApMaterno], [bTrabajador], [bIndeseado], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (2, N'80383462', N'ANITA', N'SANCHEZ', N'POLO', 0, 0, 1, CAST(0x0000AC540157A909 AS DateTime), N'ADMIN', CAST(0x0000AC540157A909 AS DateTime), N'ADMIN')
INSERT [dbo].[persona] ([idPersona], [cDocPer], [cNombres], [cApPaterno], [cApMaterno], [bTrabajador], [bIndeseado], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (3, N'47961477', N'JUNIOR ALEXANDER', N'MENDOZA', N'CHANAME', 0, 0, 1, CAST(0x0000AC54016DC914 AS DateTime), N'ADMIN', CAST(0x0000AC54016DC914 AS DateTime), N'ADMIN')
INSERT [dbo].[persona] ([idPersona], [cDocPer], [cNombres], [cApPaterno], [cApMaterno], [bTrabajador], [bIndeseado], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (4, N'25557442', N'RAFAEL', N'QUIROGA', N'ELERA', 0, 0, 1, CAST(0x0000AC5601603360 AS DateTime), N'ADMIN', CAST(0x0000AC5601603360 AS DateTime), N'ADMIN')
INSERT [dbo].[persona] ([idPersona], [cDocPer], [cNombres], [cApPaterno], [cApMaterno], [bTrabajador], [bIndeseado], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (5, N'07331668', N'MIGUEL HECTOR', N'VALDIVIA', N'BOLAÑOS', 0, 1, 1, CAST(0x0000AC5D0180C723 AS DateTime), N'ADMIN', CAST(0x0000AC5D0180C723 AS DateTime), N'ADMIN')
INSERT [dbo].[persona] ([idPersona], [cDocPer], [cNombres], [cApPaterno], [cApMaterno], [bTrabajador], [bIndeseado], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (6, N'25564331', N'ALFREDO', N'DURAND', N'BAYONA', 0, 1, 1, CAST(0x0000AC5D0184A70C AS DateTime), N'ADMIN', CAST(0x0000AC5D0184A70C AS DateTime), N'ADMIN')
INSERT [dbo].[persona] ([idPersona], [cDocPer], [cNombres], [cApPaterno], [cApMaterno], [bTrabajador], [bIndeseado], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (7, N'25798385', N'JOB', N'TREJO', N'GONZALES', 0, 0, 1, CAST(0x0000AC680131916C AS DateTime), N'ADMIN', CAST(0x0000AC680131916C AS DateTime), N'ADMIN')
INSERT [dbo].[persona] ([idPersona], [cDocPer], [cNombres], [cApPaterno], [cApMaterno], [bTrabajador], [bIndeseado], [bEstado], [dFecReg], [cUsuReg], [dFecMod], [cUsuMod]) VALUES (8, N'41429737', N'LUIS MARTIN', N'MORA', N'CUYA', 0, 1, 1, CAST(0x0000ACDB017726C9 AS DateTime), N'ADMIN', CAST(0x0000ACDB017726C9 AS DateTime), N'ADMIN')
SET IDENTITY_INSERT [dbo].[persona] OFF
SET IDENTITY_INSERT [dbo].[sucursal] ON 

INSERT [dbo].[sucursal] ([idSucursal], [cNombre], [bEstado], [dFecReg]) VALUES (1, N'IMUDESA', 1, CAST(0x0000AC4B000029D6 AS DateTime))
SET IDENTITY_INSERT [dbo].[sucursal] OFF
SET IDENTITY_INSERT [dbo].[usuario] ON 

INSERT [dbo].[usuario] ([idUsuario], [nSucursal], [cUsuNombre], [cUsuCtseña], [idTrabajador], [idArea], [bActivo], [dFecReg]) VALUES (1, 1, N'ADMIN', N'@DM1N1MUD3S@', 0, 1, 1, CAST(0x0000AC4B016FCA74 AS DateTime))
SET IDENTITY_INSERT [dbo].[usuario] OFF
ALTER TABLE [dbo].[area] ADD  CONSTRAINT [DF_area_bEstado]  DEFAULT ((1)) FOR [bEstado]
GO
ALTER TABLE [dbo].[cargaVehicular] ADD  CONSTRAINT [DF_cargaVehicular_bEstado]  DEFAULT ((1)) FOR [bEstado]
GO
ALTER TABLE [dbo].[cita] ADD  CONSTRAINT [DF_cita_bOculto]  DEFAULT ((0)) FOR [bOculto]
GO
ALTER TABLE [dbo].[cita] ADD  CONSTRAINT [DF_cita_bEstado]  DEFAULT ((1)) FOR [bEstado]
GO
ALTER TABLE [dbo].[cita] ADD  DEFAULT ((1)) FOR [nSucursal]
GO
ALTER TABLE [dbo].[cita] ADD  DEFAULT ((0)) FOR [bAtendido]
GO
ALTER TABLE [dbo].[control] ADD  DEFAULT ('normal') FOR [turno]
GO
ALTER TABLE [dbo].[empresa] ADD  CONSTRAINT [DF_empres_bEstado]  DEFAULT ((1)) FOR [bEstado]
GO
ALTER TABLE [dbo].[indeseado] ADD  CONSTRAINT [DF_indeseados_bEstado]  DEFAULT ((1)) FOR [bEstado]
GO
ALTER TABLE [dbo].[menuSistema] ADD  CONSTRAINT [DF_menuSistema_bEstado]  DEFAULT ((1)) FOR [bEstado]
GO
ALTER TABLE [dbo].[movimientoPersona] ADD  CONSTRAINT [DF_MovimientoPersona_cModoMov]  DEFAULT ('v') FOR [cModoMov]
GO
ALTER TABLE [dbo].[movimientoPersona] ADD  CONSTRAINT [DF_MovimientoPersona_bEstado]  DEFAULT ((1)) FOR [bEstado]
GO
ALTER TABLE [dbo].[persona] ADD  CONSTRAINT [DF_persona_bTrabajador]  DEFAULT ((0)) FOR [bTrabajador]
GO
ALTER TABLE [dbo].[persona] ADD  CONSTRAINT [DF_persona_bIndeseado]  DEFAULT ((0)) FOR [bIndeseado]
GO
ALTER TABLE [dbo].[persona] ADD  CONSTRAINT [DF_persona_bEstado]  DEFAULT ((1)) FOR [bEstado]
GO
ALTER TABLE [dbo].[seguro] ADD  DEFAULT ((1)) FOR [flagactivo]
GO
ALTER TABLE [dbo].[sucursal] ADD  CONSTRAINT [DF_sucursal_bEstado]  DEFAULT ((1)) FOR [bEstado]
GO
ALTER TABLE [dbo].[trabajador] ADD  DEFAULT ((1)) FOR [habilitado]
GO
ALTER TABLE [dbo].[trabajadorcuenta] ADD  DEFAULT ('') FOR [tipocuenta]
GO
ALTER TABLE [dbo].[trabajadorcuenta] ADD  DEFAULT ((1)) FOR [activo]
GO
ALTER TABLE [dbo].[usuario] ADD  CONSTRAINT [DF_usuario1_idTrabajador]  DEFAULT ((0)) FOR [idTrabajador]
GO
ALTER TABLE [dbo].[usuario] ADD  CONSTRAINT [DF_usuario1_idArea]  DEFAULT ((0)) FOR [idArea]
GO
ALTER TABLE [dbo].[usuario] ADD  CONSTRAINT [DF_usuario1_bActivo]  DEFAULT ((1)) FOR [bActivo]
GO
ALTER TABLE [dbo].[usuariosAccesos] ADD  CONSTRAINT [DF_usuariosAccesos_bAbrir]  DEFAULT ((0)) FOR [bAbrir]
GO
ALTER TABLE [dbo].[usuariosAccesos] ADD  CONSTRAINT [DF_usuariosAccesos_bGuardar]  DEFAULT ((0)) FOR [bGuardar]
GO
ALTER TABLE [dbo].[usuariosAccesos] ADD  CONSTRAINT [DF_usuariosAccesos_bCerrar]  DEFAULT ((0)) FOR [bCerrar]
GO
ALTER TABLE [dbo].[usuariosAccesos] ADD  CONSTRAINT [DF_usuariosAccesos_bExportar]  DEFAULT ((0)) FOR [bExportar]
GO
ALTER TABLE [dbo].[usuariosAccesos] ADD  CONSTRAINT [DF_usuariosAccesos_bEliminar]  DEFAULT ((0)) FOR [bEliminar]
GO
USE [master]
GO
ALTER DATABASE [IMUDESA] SET  READ_WRITE 
GO
