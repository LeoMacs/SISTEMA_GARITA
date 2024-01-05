var app = angular.module("app", []);

app.controller("myController", function ($scope, $http) {

    $scope.usuario =null;
    /****************EXTRAS MODAL**********************************/
    /**----------------------------------------------------------**/
    $scope.pushGuardar = function () {
        $scope.btntext = "ADD";
        $scope.usuario = null;
    };



 


    $scope.cerrarModalEditar = function () {
        $('#modalEdit').modal('hide');
    };



    /*------------------------------------------------------------*/
    /**************Acceso a Base de Datos**********************************/
    /*------------------------------------------------------------*/
    
    $scope.login = function () {
        var codsuc = $('#codsuc').val();

        $scope.usuario.codsuc = codsuc;
        
        $.ajax({
            type: "POST",
            url: "Home/iniciarSesion",
            data: $scope.usuario,
            success: function (respuesta) {
                if (respuesta.responseText == "SI") {
                    $scope.usuario = respuesta.usuario;
                    window.location.replace("/Home/Inicio");
                    alert("Bienvenido " + $scope.usuario.cuenta + "!!");
                }
                else {
                    alert("Datos inválidos de usuario!!");
                }

            }
        });

    };

    

    $http.get("/Home/Get_sucursales").then(function (d) {
        $scope.sucursales = d.data;
    }, function (error) {
        alert('Failed');
    });


    //load list user*************************************
    $scope.loadListusuarios = function (urldata) {
        $http.get(urldata).
            then(function (d) {
                $scope.usuarios = d.data;
            }, function (error) {
                alert('conexión fallida!!');
            });
    };


    // Add usuario*************************************
    $scope.saveusuario = function () {

        if (($scope.usuario != null)) {
            $scope.btntext = "Please Wait..";
            var url_data = "/User/Get_usuarios";
            
            $.ajax({
                type: "POST",
                url: "/User/Add_usuario",
                data: $scope.usuario,
                success: function (respuesta) {
                    var response = respuesta.responseText;
                    if (respuesta.success && (respuesta.responseText = ! "")) {
                        alert(response);
                        $scope.cerrarModalAgregar();
                        $scope.loadListusuarios(url_data);
                    } else {
                        alert("registro fallido!!");
                    }
                },
                dataType: 'json'
            });



        } else {
            alert("Complete los campos!!");
        }

    };

    // Display all Usuarios**************************
    //$http.get("/Home/Get_usuarios").
    //    then(function (d) {
    //        $scope.usuarios = d.data;
    //    }, function (error) {
    //        alert('conexión fallida!!');
    //    });



    // Display usuario by id***********************
    $scope.loadusuario = function (id) {

        $http.get("/User/Get_usuariobyid?id=" + id).then(function (d) {
            $scope.usuario = d.data[0];
        }, function (error) {

            alert('Failed');

        });

    };

    // Delete usuario*******************************
    //$scope.deleteusuario = function () {
    //    var id_usuario = parseInt($scope.idusu_delete);
    //    var url_data = "/User/Get_usuarios";
    //    $http.get("/User/delete_usuario?id=" + id_usuario).then(function (d) {
    //        $scope.cerrarModalEliminar();
    //        alert(d.data);
    //        $scope.loadListusuarios(url_data);
    //    }, function (error) {
    //        alert('error al eliminar!!');
    //    });

    //};

    // Update usuario*****************************
    //$scope.updateusuario = function () {
    //    $scope.btntext = "Please Wait..";
    //    var url_data = "/User/Get_usuarios";

    //    $.ajax({
    //        type: "POST",
    //        url: '/User/update_usuario',
    //        data: $scope.usuario,
    //        success: function (respuesta) {
    //            var response = respuesta.responseText;
    //            if (respuesta.success && (respuesta.responseText = ! "")) {
    //                alert(response);
    //                $scope.cerrarModalEditar();
    //                $scope.loadListusuarios(url_data);
    //            } else {
    //                alert("actualización fallida!!");
    //            }
    //        },
    //        dataType: 'json'
    //    });

    //};


});