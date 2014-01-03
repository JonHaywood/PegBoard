'use strict';

// Declare app level module which depends on filters, and services
angular.module('pegBoardApp', [
    'ngRoute',
    'ngCookies',
    'pegBoardApp.services',
    'pegBoardApp.controllers'
])
.config(function ($routeProvider) {
    $routeProvider
        .when('/', { controller: 'GameController', templateUrl: '/Partials/Start.html' })
        .when('/game', { controller: 'GameController', templateUrl: '/Partials/Game.html' })
        .when('/about', { templateUrl: '/Partials/About.html' })
        .otherwise({ redirectTo: '/' });
});