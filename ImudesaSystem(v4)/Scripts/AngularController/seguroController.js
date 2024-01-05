var app = angular.module("app", []);


app.controller("myController", function ($scope, $http) {


    /****************EXTRAS MODAL**********************************/
    //1: sin seguro, 2: con seguro
    $scope.pushPersonalSinSeguro = function (seguro) {
        $scope.seguro = seguro;//seguro seleccionado
        var url_data = "/Seguro/get_PersonalSinSeguro?id=" + seguro.idseguro;
        $scope.loadListTrabajadores(url_data);
        $scope.loadseguro(seguro.idseguro);
    };

    $scope.pushPersonalConSeguro = function (seguro) {
        $scope.seguro = seguro;//seguro seleccionado
        var   url_data = "/Seguro/get_PersonalxSeguro?id=" + seguro.idseguro;
        $scope.loadListTrabajadores(url_data);
        $scope.loadseguro(seguro.idseguro);
    };



    $scope.pushGuardar = function () {
        $scope.btntext = "REGISTRAR";
        accion = "ins";
        var url_data_1 = "/Empresa/Get_empresas";
        $scope.loadListempresas(url_data_1);
        $scope.loadseguro(0);

    };

    $scope.pushEditar = function (seguro) {
        $scope.btntext = "GUARDAR";
        accion = "upd";
        var url_data_1 = "/Empresa/Get_empresasconSeleccion?idempresa=" + seguro.idempresa;
        $scope.loadListempresas(url_data_1);
        $scope.loadseguro(seguro.idseguro);
    };

    $scope.pushAnular = function (seguro) {
        $scope.btntext = "DELETE";
        $scope.seguro = seguro;//variable para eliminr
    };

    $scope.cerrarModalRegMod = function () {
        $('#modalInsUpd').modal('hide');
    };

    $scope.cerrarModalDel = function () {
        $('#modalDel').modal('hide');
    };

    $scope.cerrarModalPlanillaSinSeguro = function (seguro) {
        $('#modalPersonalSinSeguro').modal('hide');
    };

    $scope.cerrarModalPlanillaConSeguro = function (seguro) {
        $('#modalPersonalSeguro').modal('hide');
    };

   
    $scope.refrescar = function () {
        $http.get("/Seguro/refresh").then(function (d) {
            $scope.refresco = d.data;
        }, function (error) {
            alert('Failed');
        });
    };


    /************************************************************************************/
    /*----------------------------------------------------------------------------------*/
    ///****************Obtener seguro*****************************///
    $scope.loadseguro = function (idseguro) {

        $http.get("/Seguro/Get_segurobyid?id=" + idseguro).then(function (d) {
            $scope.seguro = d.data[0];
        }, function (error) {
            alert('Failed');
        });
    };



    //////**************************** Mostrar todas las seguros*****************
    $http.get("/Seguro/Get_seguros").then(function (d) {
        $scope.seguros = d.data;
        $scope.numseguros = d.data.length;
    }, function (error) {
        alert('Failed');
    });


   
    /******************Lista de Seguros********************************/
    $scope.loadListseguros = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.seguros = d.data;
                $scope.numseguros = d.data.length;

            }, function (error) {
                alert('conexión fallida!!');
            });
    };

    ////**************************** DataBase empresas*****************
    $scope.loadListempresas = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.empresas = d.data;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };
    // Mostrar trabajores por seguro by id***********************
    $scope.loadListTrabajadores = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.trabajadores = d.data;
                $scope.numtrabajadores = d.data.length;

            }, function (error) {
                alert('conexión fallida!!');
            });
    };


    //***********************LISTA DE EMPRESAS DISPONIBLES SIN SEGURO*******//
    $scope.loadListEmpDisp = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.empresas = d.data;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };

    // agregar o actualizar seguro*************************************
    $scope.insupd_seguro = function (seguro) {
        $scope.seguro = seguro;
        //alert($scope.trabajador.fecIngreso);
        var url_data = "/Seguro/Get_seguros";
        ////////////////////////
        $scope.seguro.idempresa = $("#idempr").val();
        //Comprobamos que tenga formato correcto
        var fecha_aux1 = document.getElementById("fechainicio").value.split("/");
        var fechainicial = new Date(parseInt(fecha_aux1[2]), parseInt(fecha_aux1[1] - 1), parseInt(fecha_aux1[0]));
        var fecha_aux2 = document.getElementById("fechafin").value.split("/");
        var fechafinal = new Date(parseInt(fecha_aux2[2]), parseInt(fecha_aux2[1] - 1), parseInt(fecha_aux2[0]));

        if (fechainicial > fechafinal) {
            alert("La fecha de inicio de vigencia no puede ser mayor a la fecha final de vigencia, corregir!!");
            return;
        }
        ///////////////////////
        $.ajax({
            type: "POST",
            url: "/Seguro/insupd_seguro",
            data: $scope.seguro,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "SI" || response == "NO") {
                        if (response == "SI") {
                            if (accion == "ins") {
                                alert("Seguro registrado!!");
                            } else {
                                alert("Seguro actualizado!!");
                            }

                            $scope.cerrarModalRegMod();
                            $scope.loadListseguros(url_data);
                        }

                        if (response == "NO") {

                            if (accion == "ins") {
                                alert("El seguro no se registró, verifique los datos registrador!!");
                                $scope.btntext = "REGISTRAR";
                            } else {
                                alert("No se pudo actualizar los datos del seguro, verifique los datos!!");
                                $scope.btntext = "GUARDAR";
                            }

                            $scope.refrescar();
                        }
                    }
                    else {
                        alert(response);

                        if (accion == "ins") {
                            $scope.btntext = "REGISTRAR";
                        } else {
                            $scope.btntext = "GUARDAR";
                        }
                        $scope.refrescar();
                    }

                } else {
                    alert("registro fallido!!");
                }
            },
            dataType: 'json'
        });

    };

 
    /***********************************************************/

    $scope.deleteSeguro = function () {
        $scope.btntext = "Please Wait..";
        var url_data = "/Seguro/Get_seguros";

        $.ajax({
            type: "POST",
            url: "/Seguro/delete_Seguro",
            data: $scope.seguro,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "SI" || response == "NO") {
                        if (response == "SI") {
                            alert("Se ha anulado el seguro!!");
                            $scope.cerrarModalDel();
                            $scope.loadListseguros(url_data);
                        }

                        if (response == "NO") {
                            alert("No se puede anular el seguro seleccionada!!");
                            $scope.refrescar();
                        }
                    }
                    else {
                        alert(response);
                        $scope.refrescar();
                    }

                } else {
                    alert("registro fallido!!");
                }
            },
            dataType: 'json'
        });
    };

    // agregar afiliado al seguro*************************************
    $scope.add_afiliado = function (trabajador) {
        $scope.trabajador = trabajador;
        $scope.trabajador.idseguro = $scope.seguro.idseguro;
        var opcion = confirm("¿Esta seguro de afiliar al trabajador?");
        if (opcion == false) {
            return;
        }
      
        $.ajax({
            type: "POST",
            url: "/Seguro/add_afiliado",
            data: $scope.trabajador,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "SI" || response == "NO") {
                        if (response == "SI") {
                            var url_data = "/Seguro/get_PersonalSinSeguro?id=" + $scope.seguro.idseguro;
                            $scope.loadListTrabajadores(url_data);
                                alert("Trabajador afiliado!!");
                            //quitar fila
                        }

                        if (response == "NO") {
                                alert("No se pudo afiliar al trabajador!!");
                            $scope.refrescar();
                        }
                    }
                    else {
                        alert(response);
                        $scope.refrescar();
                    }

                } else {
                    alert("registro fallido!!");
                }
            },
            dataType: 'json'
        });

    };


    /*******Quitar afiliado********************************/

    $scope.del_Afiliado = function (trabajador) {
        $scope.trabajador = trabajador;
        var opcion = confirm("¿Esta seguro de desafiliar al trabajador?");
        if (opcion==false) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/Seguro/delete_afiliado",
            data: $scope.trabajador,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "SI" || response == "NO") {
                        if (response == "SI") {
                            var url_data = "/Seguro/get_PersonalxSeguro?id=" + $scope.seguro.idseguro;
                            $scope.loadListTrabajadores(url_data);
                            alert("Se desafilió al trabajador del seguro!!");

                        }

                        if (response == "NO") {
                            alert("No se puede anular el seguro seleccionada!!");
                            $scope.refrescar();
                        }
                    }
                    else {
                        alert(response);
                        $scope.refrescar();
                    }

                } else {
                    alert("registro fallido!!");
                }
            },
            dataType: 'json'
        });
    };




    /*************************************************************/

    $scope.reporteSeguros = function () {

        window.location.replace("/Reportes/reporteSCTR");

    };


    /***************************************************************/
   
    $scope.cerrarSesion = function () {
        var respuesta;
        if (confirm("Está seguro de Cerrar Sesión?")) {
            $http.get("/Home/cerrarSesion").then(function (d) {
                respuesta = d.responseText;
                if (respuesta == "SI") {
                    alert("Sesión cerrada!!");
                    window.location.replace("/Home/Login");
                }
            }, function (error) {
                alert('Failed');
            });
        }
    };




});
