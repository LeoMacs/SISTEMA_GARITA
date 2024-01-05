var app = angular.module("app", []);


app.controller("myController", function ($scope, $http) {


    /****************EXTRAS MODAL**********************************/
    $scope.pushRegMod1 = function () {
        $scope.btntext = "REGISTRAR";
        var url_data_1 = "/Empresa/Get_empresas";
        $scope.loadListempresas(url_data_1);
        var url_data_2 = "/Trabajador/Get_areas";
        $scope.loadListareas(url_data_2);
        //////////////////////////////////
        $("#dni").attr("readonly", false);
        $("#nombre").attr("readonly", false);
        $("#apPaterno").attr("readonly", false);
        $("#apMaterno").attr("readonly", false);
        //$("#fecIngreso").val(moment().format("DD/MM/YYYY"));
        $("#fec_ret").hide();
        $("#esta_trab").hide();
        accion = "ins";
        $scope.loadtrabajador(0);
    };
    /////////////////////////////////////////////
    $scope.pushRegMod2 = function (trabajador) {

        $scope.btntext = "GUARDAR";
        $("#dni").attr("readonly", true);
        $("#nombre").attr("readonly", true);
        $("#apPaterno").attr("readonly", true);
        $("#apMaterno").attr("readonly", true);
        $("#esta_trab").show();

        $scope.loadtrabajador(trabajador.idtrabajador);
        ////////////////////////////////
        var activo = trabajador.habilitado;
        if (activo == 1) {
            $("#sw_lang_left").prop('checked', true).attr('checked', 'checked');//boton si
            $("#sw_lang_right").prop('checked', false).removeAttr('checked');//boton no
            $("#fec_ret").hide();
        } else {
            $("#sw_lang_right").prop('checked', true).attr('checked', 'checked');//boton si
            $("#sw_lang_left").prop('checked', false).removeAttr('checked');//boton no
            $("#fec_ret").show();
        }
        ////////////////////////////////
        var url_data_1 = "/Empresa/Get_empresasconSeleccion?idempresa=" + trabajador.idempresa;
        $scope.loadListempresas(url_data_1);
        ////////////////////////////////
        var url_data_2 = "/Trabajador/Get_areasconSeleccion?idarea=" + trabajador.idarea;
        $scope.loadListareas(url_data_2);

       // $("#idarea").val($("#idarea option").eq(1).val());
       // $('#idarea option[value="2"]').attr("selected", true);
        //$('#idarea').val(2)
        //$('#idarea option[value="2"]').attr("selected", true);
        accion = "upd";
    };

    $scope.pushAnular = function (trabajador) {
        $scope.loadtrabajador(trabajador.idtrabajador);
    };

    $scope.cerrarModalRegMod = function () {
        $('#modalInsUpd').modal('hide');
    };


    $scope.cerrarModalDel = function () {
        $('#modalDel').modal('hide');
    };


    $scope.refrescar = function () {
        $http.get("/Trabajador/refresh").then(function (d) {
            $scope.refresco = d.data;
        }, function (error) {
            alert('Failed');
        });
    };

    //$scope.pushEditar = function (trabajador) {

    //    $scope.btntext = "GUARDAR";
    //    $scope.loadtrabajador(trabajador);
    //    var activo = trabajador.habilitado;
    //    var conSeguro = trabajador.conSeguro;

    //    var url_data = "/Trabajador/Get_empresasconSeleccion?idempresa=" + trabajador.idempresa;
    //    $scope.loadListempresas(url_data);
    //    //var fechai = $("#fecha1").val();

    //    if (activo == 1) {
    //        $("#sw_lang_left").prop('checked', true).attr('checked', 'checked');//boton si
    //        $("#sw_lang_right").prop('checked', false).removeAttr('checked');//boton no
    //    } else {
    //        $("#sw_lang_right").prop('checked', true).attr('checked', 'checked');//boton si
    //        $("#sw_lang_left").prop('checked', false).removeAttr('checked');//boton no
    //    }
    //    //alert('activo:'+activo);
    //    if (conSeguro == "SI") {
    //        $("#sw_lang_left1").prop('checked', true).attr('checked', 'checked');//boton si
    //        $("#sw_lang_right1").prop('checked', false).removeAttr('checked');//boton no
    //    } else {
    //        $("#sw_lang_right1").prop('checked', true).attr('checked', 'checked');//boton si
    //        $("#sw_lang_left1").prop('checked', false).removeAttr('checked');//boton no
    //    }

    //};

   


    ////**************************** DataBase trabajadores*****************

    // Display trabajador by id***********************
    $scope.loadtrabajador = function (id) {
        $http.get("/Trabajador/Get_trabajadorbyid?id=" + id).then(function (d) {
            $scope.trabajador = d.data[0];
        }, function (error) {
            alert('Failed');
        });
    };

    $http.get("/Trabajador/Get_trabajadores").then(function (d) {
        $scope.trabajadores = d.data;
        $scope.numtrabajadores = d.data.length;
        $scope.loadAccesos();
    }, function (error) {
        alert('Failed');
    });

  

    $scope.loadListTrabajadores = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.trabajadores = d.data;
                $scope.numtrabajadores = d.data.length;

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

    ////**************************** DataBase areas*****************
    $scope.loadListareas = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.areas = d.data;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };



    //buscar Persona*************************************
    $scope.buscarPersona = function () {
        if ($scope.trabajador.dni.length < 8) return;
        var dni = $scope.trabajador.dni;
        $http.get("/Visitante/Get_persona?dni=" + dni).then(function (d) {
            $scope.tmp_trabajador = d.data[0];
            if ($scope.tmp_trabajador.idPersona == 0) {
                alert('No hay registro del dni buscado.');
            } else {
                alert('Dni encontrado.');
                $("#dni").attr("readonly", true);
                $("#nombre").attr("readonly", true);
                $("#apPaterno").attr("readonly", true);
                $("#apMaterno").attr("readonly", true);
                $scope.trabajador.nombres = $scope.tmp_trabajador.nombres;
                $scope.trabajador.apPaterno = $scope.tmp_trabajador.apPaterno;
                $scope.trabajador.apMaterno = $scope.tmp_trabajador.apMaterno;
            }
        }, function (error) {
            alert('Failed');
        });

    };


    // agregar o actualizar visita*************************************
    $scope.insupd_trabajador = function (trabajador) {
        $scope.trabajador = trabajador;
        //alert($scope.trabajador.fecIngreso);
        var url_data = "/Trabajador/Get_trabajadores";
        ////////////////////////
        $scope.trabajador.idempresa = $("#idempr").val();
        $scope.trabajador.idarea = $("#idarea").val();
        $scope.trabajador.fecingreso = $('#fecingreso').val();
        $scope.trabajador.fecretiro = $('#fecretiro').val();
            if ($("#sw_lang_left").is(':checked')) {
                $scope.trabajador.habilitado = 1;
            } else {
                $scope.trabajador.habilitado = 0;
            }
           // alert($scope.trabajador.idempresa + "/" + $scope.trabajador.idarea + "/" + $scope.trabajador.fecingreso + "/" + $scope.trabajador.fecretiro + "/" + $scope.trabajador.habilitado);
        ////////////////////////

        $.ajax({
            type: "POST",
            url: "/Trabajador/insupd_trabajador",
            data: $scope.trabajador,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {
                        if (response == "S") {
                            if (accion == "ins") {
                                alert("Trabajador registrado!!");
                            } else {
                                alert("Trabajador actualizado!!");
                            }

                            $scope.cerrarModalRegMod();
                            $scope.loadListTrabajadores(url_data);
                        }

                        if (response == "N") {

                            if (accion == "ins") {
                                alert("El trabajador ya existe, verifique los datos registrador!!");
                                $scope.btntext = "REGISTRAR";
                            } else {
                                alert("No se pudo actualizar los datos del trabajador, verifique los datos!!");
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

    /****************************************************************/
    // anular  trabajador*****************************
    $scope.deletetrabajador = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Trabajador/Get_trabajadores";

        $.ajax({
            type: "POST",
            url: "/Trabajador/delete_trabajador",
            data: $scope.trabajador,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {
                        if (response == "S") {
                            alert("Se ha anulado el trabajador!!");
                            $scope.cerrarModalDel();
                            $scope.loadListTrabajadores(url_data);
                        }

                        if (response == "N") {
                            alert("No se puede anular el trabajador seleccionada!!");
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
    /****************************************************************/

    $scope.reporteTrabajadores = function () {
        window.location.replace("/Reportes/reporteTrabajadores");
    };


    /**************Acceso a Base de Datos**********************************/
    /*------------------------------------------------------------*/
    // ACCESOS DE USUARIO**************************
    var usuarioAccess = {
        codMenu: "V04",
        codOpcion: "02"
    };


    $scope.loadAccesos = function () {
        $scope.usuario = usuarioAccess;
        $.ajax({
            type: "POST",
            url: "/Home/Get_AccesosMenuxUsuario",
            data: $scope.usuario,
            success: function (respuesta) {
                if (respuesta.responseText == "SI") {
                    var usuario = respuesta.usuario;
                    $scope.usuario = respuesta.usuario;
                    /////////////////////////////////////
                    $("#buttonAdd").attr("disabled", getBoolean(usuario.noguardar));
                    $("#buttonExport").attr("disabled", getBoolean(usuario.noeliminar));
                    /////////////////////////////////////
                    console.log(usuario);
                }
                else {
                    alert("No se ha podido establecer los accesos del usuariol!!");
                }
            }
        });
    };


    function getBoolean(valor) {
        if (valor == "true") {
            return true;
        } else {
            return false;
        }
    };





});
