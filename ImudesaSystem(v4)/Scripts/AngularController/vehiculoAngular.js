var app = angular.module("app", []);

app.controller("myController", function ($scope, $http) {

    var accion = "";
    


    /****************EXTRAS MODAL**********************************/
    /**--------------REGISTRAR MODIFICAR----------------------------**/
    $scope.pushRegMod1 = function () {
        var url_data = "/Empresa/Get_empresas";
        $scope.loadListempresas(url_data);
        $scope.btntext = "REGISTRAR";
        $("#dni").attr("readonly", false);
        $("#nombre").attr("readonly", false);
        $("#apPaterno").attr("readonly", false);
        $("#apMaterno").attr("readonly", false);
        accion = "ins";
        $scope.loadvehiculo(0);
    };


    $scope.pushRegMod2 = function (vehiculo) {
        var url_data = "/Empresa/Get_empresasconSeleccion?idempresa=" + vehiculo.idempresa_dtno;
        $scope.loadListempresas(url_data);
        $scope.btntext = "GUARDAR";
        $("#dni").attr("readonly", true);
        $("#nombre").attr("readonly", true);
        $("#apPaterno").attr("readonly", true);
        $("#apMaterno").attr("readonly", true);
        accion = "upd";
        $scope.loadvehiculo(vehiculo.idcargavehiculo);

        var valor = vehiculo.idempresa_dtno;
        //$("#idempresa option[value=" + vehiculo.idempresa_dtno + "]").attr("selected", true);
        //$("#idempresa option[value=2").attr("selected",true);
        $("#idempresa option[value=" + valor + "]").attr("selected", true);

    };

    $scope.cerrarModalRegMod = function () {
        $('#modalInsUpd').modal('hide');
    };
    /***********REGISTRO SALIDA******************/
    $scope.pushConfirmarSalida = function (vehiculo) {
        $scope.vehiculo = vehiculo;
    };

    $scope.cerrarModalSalida = function () {
        $('#modalSalida').modal('hide');
    };
    /****************fin MODAL**********************************/
    /**************Acceso a Base de Datos**********************************/
    /*------------------------------------------------------------*/
    // ACCESOS DE USUARIO**************************
    var usuarioAccess = {
        codMenu: "V01",
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


    /********************************************/
    $scope.refrescar = function () {
        $http.get("/CargaVehicular/refresh").then(function (d) {
            $scope.refresco = d.data;
        }, function (error) {
            alert('Failed');
        });
    };

    /*------------------------------------------------------------*/
    $scope.loadListempresas = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.empresas = d.data;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };


    // Display all empresas**************************
    $http.get("/CargaVehicular/Get_cargaVehicularHoy").
      then(function (d) {
          $scope.cargas = d.data;
          $scope.numcargashoy = d.data.length;
          $scope.loadAccesos();
      }, function (error) {
          alert('conexión fallida!!');
      });


    //load list CARGAS*************************************
    $scope.loadListCargas = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.cargas = d.data;
                $scope.numcargashoy = d.data.length;
                /////////////////////////
            }, function (error) {
                alert('conexión fallida!!');
            });
    };




    // Display vehiculo by id***********************
    $scope.loadvehiculo = function (id) {
        $http.get("/CargaVehicular/Get_cargabyid?id=" + id).then(function (d) {
            $scope.vehiculo = d.data[0];
            //if ($scope.vehiculo.idcargavehiculo > 0) {
                var bunidad = $scope.vehiculo.bunidad;
                var bcarreta = $scope.vehiculo.bcarreta;
                var tipoContenedor = $scope.vehiculo.tipoContenedor;
                var estadContenedor = $scope.vehiculo.estadContenedor;

                if (bunidad == 1) {
                    document.getElementById('bunidad').checked = true;
                    $("#codcarreta").attr("readonly", true);
                } else {
                    document.getElementById('bunidad').checked = false;
                    $("#codcarreta").attr("readonly", false);
                }

                if (bcarreta == 1) {
                    document.getElementById('bcarreta').checked = true;
                } else {
                    document.getElementById('bcarreta').checked = false;
                }

                document.getElementById(tipoContenedor).click();
                document.getElementById(estadContenedor).click();
            //}

        }, function (error) {
            alert('Failed');
        });
    };


    //buscar Persona*************************************
    $scope.buscarPersona = function () {
        if ($scope.vehiculo.dni.length < 8) return;

        var dni = $scope.vehiculo.dni;
        $http.get("/Visitante/Get_persona?dni=" + dni).then(function (d) {
            $scope.tmp_v = d.data[0];
            if ($scope.tmp_v.idPersona == 0) {
                alert('No hay registro del dni buscado.');
            } else {
                $scope.vehiculo.nombres = $scope.tmp_v.nombres;
                $scope.vehiculo.apPaterno = $scope.tmp_v.apPaterno;
                $scope.vehiculo.apMaterno = $scope.tmp_v.apMaterno;
            }
        }, function (error) {
            alert('Failed');
        });

    };

    
    // ins upd visita*************************************
    $scope.InsUpd_vehiculo = function () {

        if ($('#bunidad').prop("checked") == true) {
            $scope.vehiculo.bunidad = 1;
        }
        else {
            $scope.vehiculo.bunidad = 0;
        }

        if ($('#bcarreta').prop("checked") == true) {
            $scope.vehiculo.bcarreta = 1;
        }
        else {
            $scope.vehiculo.bcarreta = 0;
            $scope.vehiculo.codcarreta = " ";
        }

        $scope.vehiculo.idempresa_dtno = $('#idempresa').val();
        $scope.vehiculo.tipoContenedor = $('#tipo').val();
        $scope.vehiculo.estadContenedor = $('#estadCarga').val();
      
        var url_data = "/CargaVehicular/Get_cargaVehicularHoy";
        $.ajax({
            type: "POST",
            url: "/CargaVehicular/insUpd_cargaVehicular",
            data: $scope.vehiculo,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {
                    if (response == "S" || response == "N") {
                        if (response == "S") {
                            if (accion == "ins") {
                                alert("Carga Vehicular registrada!!");
                            } else {
                                alert("carga Vehicular actualizada!!");
                            }
                            $scope.cerrarModalRegMod();
                            $scope.loadListCargas(url_data);
                        }

                        if (response == "N") {
                            if (accion == "ins") {
                                alert("La Carga Vehicular no se pudo registrar, primero debe registrar la salida del anterior ingreso del conductor!!");
                                $scope.btntext = "REGISTRAR";
                            } else {
                                alert("La Carga Vehicular no se pudo actualizar, verifique los datos!!");
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


    //registrar salida*****************************
    $scope.confirmarsalida = function () {
        var url_data = "/CargaVehicular/Get_cargaVehicularHoy";
        $.ajax({
            type: "POST",
            url: '/CargaVehicular/registrar_salida',
            data: $scope.vehiculo,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {
                    /********************************************/
                    if (response == "S" || response == "N") {

                        if (response == "S") {
                            alert("Salida registrada!!");
                            $scope.cerrarModalSalida();
                            $scope.loadListCargas(url_data);
                        }

                        if (response == "N") {
                            alert("No se pudo registrar la salida!!");
                            $scope.cerrarModalSalida();
                        }
                    }
                } else {
                    alert("Error:" + response);
                }
            },
            dataType: 'json'
        });

    };

    ///////////////////////**********************************************/
    ///////////////////////**********************************************/
    var primerDiaMes = moment().startOf('month').format("DD/MM/YYYY");
    var ultimoDiaMes = new moment().startOf('month').add(1, 'months').add(-1, 'days').format("DD/MM/YYYY");
    var hoy = moment().format("DD/MM/YYYY");

    $("#fechadesde").val(primerDiaMes);
    $("#fechahasta").val(hoy);
    $("#nombre").val("");
    $("#dni").val("");
    $("#licencia").val("");
    $("#placa").val("");
    $("#proviene").val("");
    $("#destino").val("");


    var parametros = {
        fechadesde: primerDiaMes,
        fechahasta: hoy,
        nombre: "",
        dni: "",
        licencia: "",
        placa: "",
        proviene: "",
        destino: ""
    };


    $http.get('/CargaVehicular/Get_VehiculosxFiltros', { params: parametros }).
        then(function (d) {
            $scope.cargasfiltradas = d.data;
            $scope.numcargas = d.data.length;
        }, function (error) {
            alert('conexión fallida!!');
        });


    
    $scope.filtrarxCampos = function () {
        var f1 = $('#fechadesde').val();
        var f2 = $('#fechahasta').val();
        var nombre = $('#nombre').val();
        var dni = $('#dni').val();
        var licencia = $('#licencia').val();
        var placa = $('#placa').val();
        var proviene = $('#proviene').val();
        var destino = $('#cliente').val();

        var params = {
            fechadesde: f1,
            fechahasta: f2,
            nombre: nombre,
            dni: dni,
            licencia: licencia,
            placa: placa,
            proviene: proviene,
            destino: destino
        };

        $http.get('/CargaVehicular/Get_VehiculosxFiltros', { params: params }).
         then(function (d) {
             $scope.cargasfiltradas = d.data;
             $scope.numcargas = d.data.length;
             alert('Búsqueda realizada!!');
             $(document).scrollTop($(document).height());0
         }, function (error) {
             alert('conexión fallida!!');
         });

    };




    //**************REPORTES**************************/
    $scope.reporteDelDia = function () {
        window.location.replace("/Reportes/ReporteCargasHoy");
    };


    $scope.reporteCarga = function () {
        var id = $scope.vehiculoedit.idcargavehiculo;
        window.location.replace("/Reportes/ReporteGetCarga" + "?id=" + id);
    };
    //**********************************************************//
    $scope.reporteFiltros = function () {
        var url_data = "/Reportes/ReporteCargasFiltros";
        var f1 = $('#fechadesde').val();
        var f2 = $('#fechahasta').val();
        var nombre = $('#nombre').val();
        var dni = $('#dni').val();
        var licencia = $('#licencia').val();
        var placa = $('#placa').val();
        var provedor = $('#proviene').val();
        var cliente = $('#cliente').val();

        /******************************** */
        window.location = url_data +
            "?fechadesde=" + f1 +
            "&fechahasta=" + f2 +
            "&nombre=" + nombre +
            "&dni=" + dni +
            "&licencia=" + licencia +
             "&placa=" + placa +
            "&proveedor=" + provedor +
            "&cliente=" + cliente
        ;
    };
    /*************************************************************/
    
    function getBoolean(valor) {
        if (valor == "true") {
            return true;
        } else {
            return false;
        }
    };


    /*************************************************************/
    //var event_bunidad = document.querySelector("input[id=bunidad]");
    //eventcheck.addEventListener('change', function () {
    //    if (this.checked) {
    //        document.getElementById('bcarreta').checked = false;
    //    } else {
    //        document.getElementById('bcarreta').checked = true;
    //    }
    //});

});