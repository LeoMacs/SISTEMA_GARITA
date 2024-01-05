var app = angular.module("app", []);

app.controller("myController", function ($scope, $http) {

    var primerDiaMes = moment().startOf('month').format("DD/MM/YYYY");
    var ultimoDiaMes = new moment().startOf('month').add(1, 'months').add(-1, 'days').format("DD/MM/YYYY");
    var hoy = moment().format("DD/MM/YYYY");

   

  
    //$("#fechadesde").val(primerDiaMes);
    //$("#fechahasta").val(ultimoDiaMes);
    //$("#nombre").val("");
    //$("#dni").val("");
    //$("#destino").val("");
    //$("#encargado").val("");


   

    /****************EXTRAS MODAL**********************************/
    /**----------------------------------------------------------**/


    //$scope.pushOcultar = function (cita) {
    //    $scope.cita_mod = cita;
    //};


    $scope.pushAnular = function (cita) {
        $scope.loadcita(cita.idcita);
    };



    $scope.pushConfirmar = function (cita) {
        $scope.loadcita(cita.idcita);
        $scope.citaTemp.nroasignado = 0;

    };

    /*******************************************************/
    $scope.cerrarModalInsUpd = function () {
        $('#modalInsUpd').modal('hide');
    };


    //$scope.cerrarModalOcultar = function () {
    //    $('#modalOcultar').modal('hide');
    //};

    $scope.cerrarModalDel = function () {
        $('#modalDel').modal('hide');
        //jquery->https://www.w3schools.com/bootstrap/bootstrap_ref_js_modal.asp
    };

    $scope.cerrarModalConfirmar = function () {
        $('#modalConfirmar').modal('hide');
    };

    $scope.refrescar = function () {
        $http.get("/Cita/refresh").then(function (d) {
            $scope.refresco = d.data;
        }, function (error) {
            alert('Failed');
        });
    };

    $scope.pushIns = function () {
        $scope.btntext = "REGISTRAR";
        var url_data = "/Empresa/Get_empresas";
        $scope.loadListempresas(url_data);
        accion = "I";
        $scope.loadcita(0);
    };

    $scope.pushUpd = function (cita) {
        $scope.btntext = "GUARDAR";
        var url_data = "/Empresa/Get_empresasconSeleccion?idempresa=" + cita.idDestino;
        $scope.loadListempresas(url_data);
        accion = "U";
        $scope.loadcita(cita.idcita);
    };

    // Display cita by id***********************
    $scope.loadcita = function (id) {
        $http.get("/Cita/Get_citabyid?id=" + id).then(function (d) {
            $scope.citaTemp = d.data[0];
        }, function (error) {
            alert('Failed');
        });
    };

    //buscar Persona*************************************
    $scope.buscarPersona = function () {
        if ($scope.citaTemp.dni.length < 8) return;
        var dni = $scope.citaTemp.dni;
        $http.get("/Visitante/Get_persona?dni=" + dni).then(function (d) {
            $scope.tmp_cita = d.data[0];
            if ($scope.tmp_cita.idPersona == 0) {
                alert('No hay registro del dni buscado.');
            } else {
                $scope.citaTemp.nombres = $scope.tmp_cita.nombres;
                $scope.citaTemp.apPaterno = $scope.tmp_cita.apPaterno;
                $scope.citaTemp.apMaterno = $scope.tmp_cita.apMaterno;
            }
        }, function (error) {
            alert('Failed');
        });

    };

    $scope.loadListempresas = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.empresas = d.data;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };


    /**************Acceso a Base de Datos**********************************/
    /*------------------------------------------------------------*/
    // ACCESOS DE USUARIO**************************
    var usuarioAccess = {
        codMenu: "V02",
        codOpcion: "01"
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
                    /////////////////////////////////////
                    $("#buttonAdd").attr("disabled", getBoolean(usuario.noguardar));
                    $("#buttonExport").attr("disabled", getBoolean(usuario.noexportar));
                    /////////////////////////////////////
                    console.log(usuario);
                }
                else {
                    alert("No se ha podido establecer los accesos del usuariol!!");
                }
            }
        });
    };

    // Display all citas**************************
    $http.get("/Cita/Get_citasDiaActual").
        then(function (d) {
            $scope.citas = d.data;
            $scope.numcitashoy = d.data.length;
            //////////////////////////////////
            $scope.citatemp = d.data[0];//--se inicializa la variable con el primer valor
            $scope.loadAccesos();
        }, function (error) {
            alert('conexión fallida!!');
        }
        );



    //load list visitas*************************************
    $scope.loadListCitas = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.citas = d.data;
                $scope.numcitasshoy = d.data.length;
                /////////////////////////
                $scope.citatemp = d.data[0];

            }, function (error) {
                alert('conexión fallida!!');
            });
    };


    $scope.InsUpdcita = function (cita) {
        $scope.citaTemp = cita;
        $scope.citaTemp.fecha = $('#fecha').val();
        $scope.citaTemp.hora = $('#hora').val();
        $scope.citaTemp.idDestino = $('#idempresa').val();
        var url_data = "/Cita/Get_citasDiaActual";
        $.ajax({
            type: "POST",
            url: "/Cita/insupd_cita",
            data: $scope.citaTemp,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {
                        if (response == "S") {
                            if (accion == "I") {
                                alert("cita registrada!!");
                            } else {
                                alert("cita actualizada!!");
                            }
                            $scope.cerrarModalInsUpd();
                            $scope.loadListCitas(url_data);
                        }

                        if (response == "N") {
                            if (accion == "I") {
                                alert("No se pudo registrar la cita!!");
                                $scope.btntext = "REGISTRAR";
                            } else {
                                alert("No se pudo actualizar la cita!!");
                                $scope.btntext = "GUARDAR";
                            }

                            $scope.refrescar();
                        }
                    }
                    else {
                        alert(response);
                        if (accion == "I") {
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
    // ocultar cita*****************************
    $scope.deletecita = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Cita/Get_citasDiaActual";

        $.ajax({
            type: "POST",
            url: "/cita/delete_cita",
            data: $scope.citaTemp,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {
                        if (response == "S") {
                            alert("Se ha anulado la cita!!");
                            $scope.cerrarModalDel();
                            $scope.loadListCitas(url_data);
                        }

                        if (response == "N") {
                            alert("No se puede anular la cita seleccionada!!");
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
    // ocultar cita*****************************
    $scope.confirmarAsistencia = function () {
        
        $scope.btntext = "Please Wait..";
        var url_data = "/Cita/Get_citasDiaActual";

        $.ajax({
            type: "POST",
            url: "/cita/confirm_AsistenciaCita",
            data: $scope.citaTemp,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {
                        if (response == "S") {
                            alert("Asistencia confirmada!!");
                            $scope.cerrarModalConfirmar();
                            $scope.loadListCitas(url_data);
                        }

                        if (response == "N") {
                            alert("El número ya está asignado, primero registre la salida del visitante que se le asignó el número!!");
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


    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////

    $scope.parametros = {
        fechadesde: primerDiaMes,
        fechahasta: ultimoDiaMes,
        nombre: "",
        dni: "",
        destino: "",
        encargado: ""
    };

    $http.get('/Cita/Get_citasxFiltros', { params: $scope.parametros }).
        then(function (d) {
            $scope.citasfiltradas = d.data;
            $scope.numcitas = d.data.length;
        }, function (error) {
            alert('conexión fallida!!');
        });


    $scope.filtrarxCampos = function () {
        //var f1 = $('#fechadesde').val();
        //var f2 = $('#fechahasta').val();
        //var nombre = $('#nombre').val();
        //var dni = $('#dni').val();
        //var destino = $('#destino').val();
        //var encargado = $('#encargado').val();

        //var params = {
        //    fechadesde: f1,
        //    fechahasta: f2,
        //    nombre: nombre,
        //    dni: dni,
        //    destino: destino,
        //    encargado: encargado
        //};

        $http.get('/Cita/Get_citasxFiltros', { params: $scope.parametros }).
            then(function (d) {
                $scope.citasfiltradas = d.data;
                $scope.numcitas = d.data.length;
                alert("Búsqueda realizada!!");

            }, function (error) {
                alert('conexión fallida!!');
            });

    };




    //***********************************************************/
    $scope.reporteDelDia = function () {

        window.location.replace("/Reportes/ReporteCitasHoy");

    };
    //**********************************************************//
    $scope.reporteFiltros = function () {
        var url_data = "/Reportes/ReporteCitasFiltros";
        var f1 = $('#fechadesde').val();
        var f2 = $('#fechahasta').val();
        var nombre = $('#nombre').val();
        var dni = $('#dni').val();
        var destino = $('#destino').val();
        var encargado = $('#encargado').val();

        /******************************** */
        window.location = url_data +
            "?fechadesde=" + f1 +
            "&fechahasta=" + f2 +
            "&nombre=" + nombre +
            "&dni=" + dni +
             "&encargado=" + encargado +
            "&destino=" + destino

        ;
    };
    /*************************************************************/
    //// ocultar cita*****************************
    //$scope.ocultarcita = function () {

    //    $scope.btntext = "Please Wait..";
    //    var url_data = "/Cita/Get_citasDiaActual";

    //    $.ajax({
    //        type: "POST",
    //        url: "/cita/ocultar_cita",
    //        data: $scope.cita_mod,
    //        success: function (respuesta) {
    //            var response = respuesta.responseText;
    //            if (respuesta.success && (respuesta.responseText != "")) {

    //                if (response == "SI" || response == "NO") {
    //                    if (response == "SI") {
    //                        alert("cita ocultada!!");
    //                        $scope.cerrarModalAnular();
    //                        $scope.loadListCitas(url_data);
    //                    }

    //                    if (response == "NO") {
    //                        alert("No se puede ocultar la cita seleccionada!!");
    //                        $scope.refrescar();
    //                    }
    //                }
    //                else {
    //                    alert(response);
    //                    $scope.refrescar();
    //                }

    //            } else {
    //                alert("registro fallido!!");
    //            }
    //        },
    //        dataType: 'json'
    //    });

    //};
    /*************************************************************/


    function getBoolean(valor) {
        if (valor == "true") {
            return true;
        } else {
            return false;
        }
    };


});