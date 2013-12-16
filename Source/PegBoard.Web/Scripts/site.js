// Controller for navigation elements and selecting the currently active view
function NavController($scope, $location) {
    $scope.isActive = function (viewLocation) {
        return viewLocation === $location.path();
    };
}

// The main peg board app
var pegBoardApp = angular.module('pegBoardApp', ['ngCookies']);

pegBoardApp.config(function ($routeProvider) {
    $routeProvider
        .when('/', { controller: 'GameController', templateUrl: '/Templates/Start.html' })
        .when('/game', { controller: 'GameController', templateUrl: '/Templates/Game.html' })
        .when('/about', { templateUrl: '/Templates/About.html' })
        .otherwise({ redirectTo: '/' });
});

pegBoardApp.factory('userStore', function ($cookieStore) {
    var currentUser = {
        nickname: undefined,
        avatarUrl: undefined,
        registered: false
    };

    return {
        get: function () {
            var userFromCookie = $cookieStore.get('currentUser');
            // if the current user isn't set but exists in the cookie then load it
            if (!currentUser.registered && userFromCookie)
                currentUser = userFromCookie;
            return currentUser;
        },
        save: function (user) {
            user.registered = true;
            $cookieStore.put('currentUser', user);
        }
    };
});

pegBoardApp.controller('RegisterController', function ($scope, $location, userStore) {
    $scope.user = userStore.get();

    $scope.register = function () {
        userStore.save($scope.user);
        $location.path('game');
    };

    // on first load check to see if we've registered before
    // if so just go to the game
    if ($scope.user.registered) {
        $location.path('game');
        return;
    }
});

pegBoardApp.controller('GameController', function ($scope, $http, $location, userStore) {
    // make sure we've gone through the register step
    var user = userStore.get();
    if (!user.registered) {
        $location.path('/');
        return;
    }

    $scope.playing = false;
    $scope.paused = false;
    $scope.startTime = undefined;
    $scope.prevTimeDiff = 0;
    $scope.ellapsedTime = undefined;
    $scope.timer = undefined;
    $scope.pegCount = 15;
    $scope.coords = [
        { boardX: 1, boardY: 0, screenX: 213, screenY: 33, hasPeg: true },
        { boardX: 0, boardY: 1, screenX: 316, screenY: 33, hasPeg: true },
        { boardX: 2, boardY: 0, screenX: 145, screenY: 100, hasPeg: true },
        { boardX: 5, boardY: 0, screenX: 58, screenY: 260, hasPeg: true }
    ];

    function placePegs() {        
        $.each($scope.coords, function (index, coord) {
            var id = 'coord_' + coord.boardX + '_' + coord.boardY;
            var coordExists = !!($(id).length);

            // if this peg hasn't been created yet then build it and add it to the DOM
            if (!coordExists) {                
                $('<div />', { 'class': 'coord', id: id }).css({ top: coord.screenY, left: coord.screenX }).appendTo("#coords");
            }

            var $coord = $('#' + id);            
            if (coord.hasPeg)
                $coord.addClass('hasPeg');
            else
                $coord.removeClass('hasPeg');
        });
    }
    
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

    // pause the game
    $scope.pause = function () {
        $scope.prevTimeDiff += new Date() - $scope.startTime;        
        $scope.paused = true;
        clearInterval($scope.timer);
    };

    // stop the game
    $scope.stop = function() {
        $scope.playing = false;
        $scope.paused = false;
        
        // stop the timer
        clearInterval($scope.timer);
        $scope.prevTimeDiff = 0;
        $scope.ellapsedTime = '00:00:00';        
        $scope.pegCount = 15;        
    };

    placePegs();
});