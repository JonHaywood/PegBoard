// Controller for navigation elements and selecting the currently active view
function NavController($scope, $location) {
    $scope.isActive = function (viewLocation) {
        return viewLocation === $location.path();
    };
}

// The main peg board app
var pegBoardApp = angular.module('pegBoardApp', []);

pegBoardApp.config(function ($routeProvider) {
    $routeProvider
        .when('/', { controller: 'GameController', templateUrl: '/Templates/Game.html' })
        .when('/about', { templateUrl: '/Templates/About.html' })
        .otherwise({ redirectTo: '/' });
});

pegBoardApp.controller('GameController', function ($scope, $http, $location) {
   
});