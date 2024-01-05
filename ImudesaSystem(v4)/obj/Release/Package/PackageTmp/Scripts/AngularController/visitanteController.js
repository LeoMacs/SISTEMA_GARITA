var app = angular.module("app", []);

app.controller("myController", function ($scope, $http) {

    var accion = "";
    var Modulo = "PVI.OPC1";
    var primerDiaMes = moment().startOf('month').format("DD/MM/YYYY");
    var ultimoDiaMes = new moment().startOf('month').add(1, 'months').add(-1, 'days').format("DD/MM/YYYY");

    $("#fechadesde").val(primerDiaMes);
    $("#fechahasta").val(ultimoDiaMes);
    $("#nombre").val("");
    $("#dni").val("");
    $("#destino").val("");
    $("#encargado").val("");
    $("#proveedor").val("");


    /****************EXTRAS MODAL**********************************/
    /**----------------------------------------------------------**/
    $scope.pushRegMod1 = function () {
        $scope.btntext = "REGISTRAR";
        $("#dni").attr("readonly", false);
        $("#nombre").attr("readonly", false);
        $("#apPaterno").attr("readonly", false);
        $("#apMaterno").attr("readonly", false);
        accion = "ins";
        $scope.loadvisita(0);

    };

    $scope.pushRegMod2 = function (visitante) {
        $scope.btntext = "GUARDAR";
        $("#dni").attr("readonly", true);
        $("#nombre").attr("readonly", true);
        $("#apPaterno").attr("readonly", true);
        $("#apMaterno").attr("readonly", true);
        accion = "upd";
        $scope.loadvisita(visitante.idMovimiento);
    };

    $scope.cerrarModalRegMod = function () {
        $('#modalInsUpd').modal('hide');
    };

    $scope.pushEliminar = function (visitante) {
        $scope.btntext = "DELETE";
        $scope.idusu_delete = visitante.idvisitante;//variable para eliminr
    };

    $scope.cerrarModalEliminar = function () {
        $('#modalDelete').modal('hide');
        //jquery->https://www.w3schools.com/bootstrap/bootstrap_ref_js_modal.asp
    };


    $scope.refrescar = function () {
        $http.get("/Visitante/refresh").then(function (d) {
            $scope.refresco = d.data;
        }, function (error) {
            alert('Failed');
        });
    };

    $scope.pushConfirmarSalida = function (visitante) {
        $scope.visita = visitante;
    };

    $scope.cerrarModalSalida = function () {
        $('#modalSalida').modal('hide');
    };


    /****************fin MODAL**********************************/


    /**************Acceso a Base de Datos**********************************/
    /*------------------------------------------------------------*/
    // Display all visitas**************************
    $http.get("/Visitante/Get_visitantes_DiaActual").
        then(function (d) {
            $scope.visitas = d.data;
            $scope.numvisitashoy = d.data.length;

        }, function (error) {
            alert('conexión fallida!!');
        });


    // Display visita by id***********************
    $scope.loadvisita = function (id) {
        $http.get("/Visitante/Get_visitabyid?id=" + id).then(function (d) {
            $scope.visita = d.data[0];
        }, function (error) {
            alert('Failed');
        });
    };


    //load list visitas*************************************
    $scope.loadListvisitas = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.visitas = d.data;
                $scope.numvisitashoy = d.data.length;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };



    //buscar Persona*************************************
    $scope.buscarPersona = function () {
        if ($scope.visita.dni.length < 8) return;

        var dni = $scope.visita.dni;
        $http.get("/Visitante/Get_persona?dni=" + dni).then(function (d) {
            $scope.tmp_visita = d.data[0];
            if ($scope.tmp_visita.idPersona == 0) {
                alert('No hay registro del dni buscado.');
            } else {
                $scope.visita.nombres = $scope.tmp_visita.nombres;
                $scope.visita.apPaterno = $scope.tmp_visita.apPaterno;
                $scope.visita.apMaterno = $scope.tmp_visita.apMaterno;
            }
        }, function (error) {
            alert('Failed');
        });

    };

 

    // agregar o actualizar visita*************************************
    $scope.insupd_visita = function (visita) {
        $scope.visita = visita;
        var url_data = "/Visitante/Get_visitantes_DiaActual";
        $.ajax({
            type: "POST",
            url: "/Visitante/insupd_visita",
            data: $scope.visita,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {
                        if (response == "S") {
                            if (accion == "ins") {
                                alert("Visita registrada!!");
                            } else {
                                alert("Visita actualizada!!");
                            }

                            $scope.cerrarModalRegMod();
                            $scope.loadListvisitas(url_data);
                        }

                        if (response == "N") {

                            if (accion == "ins") {
                                alert("El visitante ya tiene ingreso o el número ya está asignado, primero registre la salida del visitante o número para registrar su ingreso de nuevo!!");
                                $scope.btntext = "REGISTRAR";
                            } else {
                                alert("El número ya está asignado, primero registre la salida del visitante con ese número!!");
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

    ///****************marcar salida*******************************///
    $scope.marcarsalida = function () {
        var url_data = "/Visitante/Get_visitantes_DiaActual";

        $.ajax({
            type: "POST",
            url: '/Visitante/registrar_salida',
            data: $scope.visita,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {
                    /********************************************/
                    if (response == "S" || response == "N") {

                        if (response == "S") {
                            alert("Salida registrada!!");
                            $scope.cerrarModalSalida();
                            $scope.loadListvisitas(url_data);
                        }

                        if (response == "N") {
                            alert("Error al registrar la salida!!");
                            $scope.cerrarModalSalida();

                        }

                    }

                } else {
                    alert("registro de salida fallida!!");
                }
            },
            dataType: 'json'
        });

    };
    /******************BUSQUEDA FILTRADA*******************************/
    var parametros = {
        fechadesde: primerDiaMes,
        fechahasta: ultimoDiaMes,
        nombre: "",
        dni: "",
        destino: "",
        encargado: "",
        proveedor: ""
    };


    $http.get("/Visitante/Get_visitantesxFiltros", { params: parametros }).
    then(function (d) {
        //$scope.visitasfiltradas = d.data;
        //$scope.numvisitas = d.data.length;
        $scope.visitasfiltradas = d.data.visitas;
        $scope.numvisitas = d.data.visitas.length;
    }, function (error) {
        alert('conexión fallida!!');
    });



    $scope.filtrarxCampos = function () {
        var f1 = $('#fechadesde').val();
        var f2 = $('#fechahasta').val();
        var nombre = $('#nombre').val();
        var dni = $('#dni').val();
        var destino = $('#destino').val();
        var encargado = $('#encargado').val();
        var proveedor = $('#proveedor').val();


        var params = {
            fechadesde: f1,
            fechahasta: f2,
            nombre: nombre,
            dni: dni,
            destino: destino,
            encargado: encargado,
            proveedor: proveedor
        };

        $http.get('/Visitante/Get_visitantesxFiltros', { params: params }).
        then(function (d) {
            $scope.visitasfiltradas = d.data.visitas;
            $scope.numvisitas = d.data.visitas.length;
            alert(d.data.responseText);
            $(document).scrollTop($(document).height());
        }, function (error) {
            alert(d.data.responseText);
        });

    };


    //// Delete visita*******************************
    //$scope.deletevisita = function () {
    //    var id_visita = parseInt($scope.idvis_delete);
    //    var url_data = "/Visitante/Get_Visitas";
    //    $http.get("/Visitante/delete_visitante?id=" + id_visita).then(function (d) {
    //        $scope.cerrarModalEliminar();
    //        alert(d.data);
    //        $scope.loadListvisitas(url_data);
    //    }, function (error) {
    //        alert('error al eliminar!!');
    //    });

    //};

    //***********************************************************/
    //$scope.reporteDelDia = function () {

    //    window.location.replace("/Reportes/ReporteVisitasHoy");

    //};
    //**********************************************************//
    //$scope.reporteFiltros = function () {
    //    var url_data = "/Reportes/ReporteVisitasFiltros";
    //    var f1 = $('#fechadesde').val();
    //    var f2 = $('#fechahasta').val();
    //    var nombre = $('#nombre').val();
    //    var dni = $('#dni').val();
    //    var destino = $('#destino').val();
    //    var proveedor = $('#proveedor').val();

    //    /******************************** */
    //    window.location = url_data +
    //        "?fechadesde=" + f1 +
    //        "&fechahasta=" + f2 +
    //        "&nombre=" + nombre +
    //        "&dni=" + dni +
    //        "&destino=" + destino +
    //        "&proveedor=" + proveedor
    //    ;
    //};
    /*************************************************************/




});