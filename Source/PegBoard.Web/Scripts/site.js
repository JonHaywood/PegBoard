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
        .when('/', { controller: 'GameController', templateUrl: '/Templates/Start.html' })
        .when('/game', { controller: 'GameController', templateUrl: '/Templates/Game.html' })
        .when('/about', { templateUrl: '/Templates/About.html' })
        .otherwise({ redirectTo: '/' });
});

pegBoardApp.controller('RegisterController', function($scope, $location) {
    $scope.registered = false;

    $scope.register = function() {
        $scope.registered = true;
        $location.path('game');
    };
    $scope.skip = function() {
        $scope.registered = true;
        $location.path('game');
    };
});

pegBoardApp.controller('GameController', function ($scope, $http, $location) {
    $scope.playing = false;
    $scope.paused = false;
    $scope.startTime = undefined;
    $scope.prevTimeDiff = undefined;
    $scope.ellapsedTime = undefined;
    $scope.timer = undefined;
    $scope.pegCount = 15;
    
    // see http://stackoverflow.com/questions/10073699/pad-a-number-with-leading-zeros-in-javascript
    function pad(n, width, z) {
        z = z || '0';
        n = n + '';
        return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
    }

    // see http://stackoverflow.com/questions/1210701/compute-elapsed-time
    function getEllapsedTime() {
        var prevTimeDiff = $scope.prevTimeDiff || 0;

        // later record end time
        var endTime = new Date();

        // time difference in ms
        var timeDiff = (endTime - $scope.startTime) + prevTimeDiff;

        // strip the miliseconds
        timeDiff /= 1000;

        // get seconds
        var seconds = Math.round(timeDiff % 60);

        // remove seconds from the date
        timeDiff = Math.floor(timeDiff / 60);

        // get minutes
        var minutes = Math.round(timeDiff % 60);

        // remove minutes from the date
        timeDiff = Math.floor(timeDiff / 60);

        // get hours
        var hours = Math.round(timeDiff % 24);

        // remove hours from the date
        timeDiff = Math.floor(timeDiff / 24);

        // the rest of timeDiff is number of days
        var days = timeDiff;

        var timeStr = pad(hours, 2) + ":" + pad(minutes, 2) + ":" + pad(seconds, 2);
        if (days > 0)
            timeStr = days + " days, " + timeStr;

        return timeStr;
    }    

    // start the game!
    $scope.start = function () {
        $scope.startTime = new Date();
        $scope.paused = false;
        $scope.playing = true;
        
        // start the timer
        $scope.timer = setInterval(function() {
            $scope.ellapsedTime = getEllapsedTime();
            $scope.$apply();
        }, 1000);       
    };

    $scope.pause = function () {
        $scope.prevTimeDiff = new Date() - $scope.startTime;
        $scope.paused = true;
        clearInterval($scope.timer);
    };

    // stop the game
    $scope.stop = function() {
        $scope.playing = false;
        $scope.paused = false;
        
        // stop the timer
        clearInterval($scope.timer);
        $scope.ellapsedTime = '00:00:00';        
        $scope.pegCount = 15;
        $scope.$apply();
    };
});