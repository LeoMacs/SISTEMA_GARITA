var app = angular.module("app", []);


app.controller("myController", function ($scope, $http) {

    /****************----EXTRAS MODAL-----------**********************************/
    /****************MODAL MODIFICAR AGREGAR INDESEADO**********************************/
    $scope.pushRegMod1 = function () {
        $scope.btntext = "REGISTRAR";
        $("#dni").attr("readonly", false);
        $("#nombre").attr("readonly", false);
        $("#apPaterno").attr("readonly", false);
        $("#apMaterno").attr("readonly", false);
        accion = "ins";
        $scope.loadindeseado(0);
    };

    $scope.pushRegMod2 = function (indeseado) {
        $scope.btntext = "GUARDAR";
        $("#dni").attr("readonly", true);
        $("#nombre").attr("readonly", true);
        $("#apPaterno").attr("readonly", true);
        $("#apMaterno").attr("readonly", true);
        accion = "upd";
        $scope.loadindeseado(indeseado.idindeseado);
    };

   
    $scope.cerrarModalRegMod = function () {
        $("#modalInsUpd").modal("hide");
    };

    /****************MODAL ANULAR INDESEADO**********************************/

    $scope.pushAnular = function (indeseado) {
        $scope.loadindeseado(indeseado.idindeseado);
    };

    $scope.cerrarModalAnular = function () {
        $('#modalAnular').modal('hide');
    };


    /****************REFRESCAR**********************************/
    $scope.refrescar = function () {
        $http.get("/Indeseados/refresh").then(function (d) {
            $scope.refresco = d.data;
        }, function (error) {
            alert('Failed');
        });
    };

    ////**************************** DataBase indeseados*****************
    // ACCESOS DE USUARIO**************************
    var usuarioAccess = {
        codMenu: "V01",
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

   

    // Display all indeseados**************************
    $http.get("/Indeseados/Get_indeseados").then(function (d) {
        $scope.indeseados = d.data;
        $scope.numindesdeados = d.data.length;
        $scope.loadAccesos();
    }, function (error) {
        alert('Failed');
    });

    // Mostrar INDEDESEADO by id***********************
    $scope.loadindeseado = function (id) {
        $http.get("/Indeseados/Get_indeseadobyid?id=" + id).then(function (d) {
            $scope.indeseado = d.data[0];
        }, function (error) {
            alert('Failed');
        });
    };


    $scope.loadListIndeseados = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.indeseados = d.data;
                $scope.numindesdeados = d.data.length;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };



    //buscar Persona*************************************
    $scope.buscarPersona = function () {
        if ($scope.indeseado.dni.length < 8) return;
        var dni = $scope.indeseado.dni;
        $http.get("/Visitante/Get_persona?dni=" + dni).then(function (d) {
            $scope.tmp_indeseado = d.data[0];
            if ($scope.tmp_indeseado.idPersona == 0) {
                alert('No hay registro del dni buscado.');
            } else {
                $scope.indeseado.nombres = $scope.tmp_indeseado.nombres;
                $scope.indeseado.apPaterno = $scope.tmp_indeseado.apPaterno;
                $scope.indeseado.apMaterno = $scope.tmp_indeseado.apMaterno;
            }
        }, function (error) {
            alert('Failed');
        });
    };


    ///****************ins upd Indeseado*******************************///
    $scope.insupd_indeseado = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Indeseados/Get_indeseados";

        $.ajax({
            type: "POST",
            url: "/Indeseados/insupd_indeseado",
            data: $scope.indeseado,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {
                    if (response == "S" || response == "N") {

                        if (response == "S") {
                            if (accion == "ins") {
                                alert("visitante registrado en la lista negra!!");
                            } else {
                                alert("Se actualizaron los datos!!");
                            }

                            $scope.cerrarModalRegMod();
                            $scope.loadListIndeseados(url_data);
                        }

                        if (response == "N") {
                            if (accion == "ins") {
                                alert("Visitante ya existe en la Lista Negra!!");
                                $scope.btntext = "REGISTRAR";
                            } else {
                                alert("No se pudieron actualizar los datos");
                                $scope.btntext = "GUARDAR";
                            }

                            $scope.refrescar();
                            
                        }
                    }
                    else {
                        alert(response);//muestra el mensaje de error
                        if (accion == "ins") {
                            $scope.btntext = "REGISTRAR";
                        } else {
                            $scope.btntext = "GUARDAR";
                        }
                        $scope.refrescar();
                    }
                } else {
                    alert("Registro fallido.");
                }
            },
            dataType: 'json'
        });
    };
    
    /****************************************************************/
    // eliminar indeseado*****************************
    $scope.anularIndeseado = function () {

        $scope.btntext = "Please Wait..";
        var url_data = "/Indeseados/Get_indeseados";

        $.ajax({
            type: "POST",
            url: "/Indeseados/delete_indeseado",
            data: $scope.indeseado,
            success: function (respuesta) {
                var response = respuesta.responseText;
                if (respuesta.success && (respuesta.responseText != "")) {

                    if (response == "S" || response == "N") {
                        if (response == "S") {
                            alert("Se ha anulado a la persona de la lista Negra!!");
                            $scope.cerrarModalAnular();
                            $scope.loadListIndeseados(url_data);
                        }

                        if (response == "N") {
                            alert("No se puede anular a la persona seleccionada!!");
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
    $scope.reporteindeseados = function () {
        window.location.replace("/Reportes/ReporteIndeseados");
    };
    
    /*************************************************************/
    function getBoolean(valor) {
        if (valor == "true") {
            return true;
        } else {
            return false;
        }
    };





});
