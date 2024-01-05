var app = angular.module("app", []);


app.controller("myController", function ($scope, $http) {

    var idperfil = 0;

    $http.get("/Home/Get_usuarioEnSesión").then(function (d) {
        $scope.usuario = d.data[0];
        idperfil = $scope.usuario.idperfil;
    }, function (error) {
        alert('Failed');
    });


    // Mostrar asistencias***********************
    $scope.habilitarBotones = function (opcion) {
        if (opcion == 1) {
            window.location.replace("/Cita/PanelCitas");
        }
        if (opcion == 2) {
            if (idperfil > 2) {
                alert('El usuario no tiene acceso a este módulo!!');
            } else {
                window.location.replace("/Visitante/PanelVisitantes");
            }
        }
        if (opcion == 3) {
            if (idperfil > 2) {
                alert('El usuario no tiene acceso a este módulo!!');
            } else {
                window.location.replace("/Empresa/panelEmpresas");
            }
        }
        if (opcion == 4) {
            if (idperfil > 2) {
                alert('El usuario no tiene acceso a este módulo!!');
            } else {
                window.location.replace("/CargaVehicular/PanelVehiculos");
            }
        }
        
    };



});
