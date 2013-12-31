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

pegBoardApp.factory('timerService', function ($rootScope) {
    return {
        // the ellapsed time
        ellapsedTime: '00:00:00',

        // ellapsed time from a previous timer
        prevTimeDiff: 0,

        // the time the time service was started
        startTime: undefined,

        // handle to the timer
        timer: undefined,

        // sets the start time for the service
        start: function() {
            var self = this;

            // set the start time
            self.startTime = new Date();

            // start the timer
            self.timer = setInterval(function () {                
                self.ellapsedTime = self.getEllapsedTime();
                $rootScope.$apply();
            }, 1000);
        },

        // pause the timer
        pause: function () {
            var self = this;
            
            self.prevTimeDiff += new Date() - self.startTime;            
            clearInterval(self.timer);
        },

        // stops the timer completely
        stop: function () {
            var self = this;

            clearInterval(self.timer);
            self.prevTimeDiff = 0;
            self.ellapsedTime = '00:00:00';
        },

        // see http://stackoverflow.com/questions/10073699/pad-a-number-with-leading-zeros-in-javascript
        pad: function(n, width, z) {
            z = z || '0';
            n = n + '';
            return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
        },

        // parses the time diff into days, hours, minutes and seconds
        parseTimeDiff: function(timeDiff) {
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

            return {
                days: days,
                hours: hours,
                minutes: minutes,
                seconds: seconds
            };
        },

        // see http://stackoverflow.com/questions/1210701/compute-elapsed-time
        getEllapsedTime: function () {
            var self = this;
            var prevTimeDiff = self.prevTimeDiff || 0;

            // later record end time
            var endTime = new Date();

            // time difference in ms
            var timeDiff = (endTime - self.startTime) + prevTimeDiff;            

            // parse the ellapsed time
            var ellapsed = self.parseTimeDiff(timeDiff);

            // format the time in a readable string
            var timeStr = self.pad(ellapsed.hours, 2) + ":" + self.pad(ellapsed.minutes, 2) + ":" + self.pad(ellapsed.seconds, 2);
            if (ellapsed.days > 0)
                timeStr = ellapsed.days + " days, " + timeStr;

            return timeStr;
        }
    };
});

pegBoardApp.factory('boardService', function ($rootScope) {    
    var coords = [
        { boardX: 0, boardY: 0, screenX: 273, screenY: -37, hasPeg: false },
        { boardX: 1, boardY: 0, screenX: 223, screenY: 30, hasPeg: true },
        { boardX: 0, boardY: 1, screenX: 322, screenY: 32, hasPeg: true },
        { boardX: 2, boardY: 0, screenX: 165, screenY: 105, hasPeg: true },
        { boardX: 1, boardY: 1, screenX: 265, screenY: 105, hasPeg: true },
        { boardX: 0, boardY: 2, screenX: 370, screenY: 105, hasPeg: true },
        { boardX: 3, boardY: 0, screenX: 110, screenY: 183, hasPeg: true },
        { boardX: 2, boardY: 1, screenX: 217, screenY: 183, hasPeg: true },
        { boardX: 1, boardY: 2, screenX: 325, screenY: 183, hasPeg: true },
        { boardX: 0, boardY: 3, screenX: 425, screenY: 183, hasPeg: true },
        { boardX: 4, boardY: 0, screenX: 58, screenY: 260, hasPeg: true },
        { boardX: 3, boardY: 1, screenX: 165, screenY: 260, hasPeg: true },
        { boardX: 2, boardY: 2, screenX: 268, screenY: 262, hasPeg: true },
        { boardX: 1, boardY: 3, screenX: 375, screenY: 265, hasPeg: true },
        { boardX: 0, boardY: 4, screenX: 480, screenY: 265, hasPeg: true }
    ];
    var showCoords = false;
    var xs = [ -1, -1,  0,  1, 1, 1, 0, -1 ];
    var ys = [  0, -1, -1, -1, 0, 1, 1,  1 ];    

    return {
        defaultCount: 14,

        getPeg: function (x, y) {
            var pegs = $.grep(coords, function (coord, i) { return (coord.boardX == x && coord.boardY == y); });
            return pegs.pop();
        },

        getCount: function () {
            var pegs = $.grep(coords, function (coord, i) { return coord.hasPeg; });
            return pegs.length;
        },

        placePegs: function () {
            var self = this;

            $.each(coords, function (index, coord) {
                var id = 'coord_' + coord.boardX + '_' + coord.boardY;
                var $coord = $('#' + id);
                var coordExists = !!($coord.length);

                // if this peg hasn't been created yet then build it and add it to the DOM
                if (!coordExists)
                    $coord = self.createPegElement(coord, id);

                // set the correct class if the coord has a peg or not
                if (coord.hasPeg)
                    $coord.addClass('hasPeg');
                else
                    $coord.removeClass('hasPeg');
            });
        },

        removeHighlights: function () {
            $('.peg-highlight').removeClass('peg-highlight').droppable('destroy');
        },

        createPegElement: function (coord, id) {
            var self = this;
            var droppables = [];

            // create the peg element and append it to the dom
            $('<div />', { 'class': 'coord', id: id }).css({ top: coord.screenY, left: coord.screenX }).appendTo("#coords");

            // get a reference to the element and set up events on it
            $el = $('#' + id);
            $el.data('x', coord.boardX);
            $el.data('y', coord.boardY);
            if (showCoords) {
                $el.append('<div class="peg-data">x: ' + coord.boardX + ', y: ' + coord.boardY + '</div>');
            }
            $el.draggable({
                containment: '#board-container',
                cursor: "move",
                helper: function (event) {
                    return $("<img src='/Content/game/peg-full.png' />");
                },
                start: function (event, ui) {
                    // hide the original position of peg being dragged                    
                    coord.el.hide();
                                        
                    // get all coordinates which are valid
                    var validJumps = self.getAvailableJumps(coord);
                    if (!validJumps || validJumps.length == 0) {
                        console.log('No valid jumps.');
                        return;
                    }

                    // make each valid jump location droppable
                    $.each(validJumps, function (index, validJump) {
                        var target = validJump.target;
                        var jumped = validJump.jumped;
                        var coord = self.getPeg(target.x, target.y);

                        coord.el.addClass('peg-highlight').droppable({
                            //accept: function (el) {
                            //    debugger;
                            //    /* This is a filter function, you can perform logic here 
                            //       depending on the element being filtered: */
                            //    return el.hasClass('peg-highlight');
                            //},
                            // occurs when the peg is dropped successfully
                            drop: function (event, ui) {
                                self.getPeg(jumped.x, jumped.y).hasPeg = false; // set jumped peg to false
                                coord.hasPeg = true;                            // set where peg jumps to as true                                
                                self.removeHighlights();                                
                            }
                        });
                    });
                },
                stop: function (event, ui) {

                },
                revert: function (droppableObj) {
                    // see http://stackoverflow.com/questions/1853230/jquery-ui-draggable-event-status-on-revert
                    // will be false if not placed on a droppable
                    if (droppableObj === false) {
                        coord.el.show();
                        self.removeHighlights();
                        return true;
                    } else {
                        coord.hasPeg = false; // set where the peg jumped from as false
                        self.placePegs();     // draw pegs on the screen
                        coord.el.show();      // reshow the previously hidden div (since it may have a peg)
                        $rootScope.$apply();  // update the root scope to notify controllers/views of a change

                        // when the game is over notify watchers
                        if (self.isGameOver()) {
                            $rootScope.$apply();
                            $('#game-over-screen').fadeIn('slow');
                        }

                        //return false so that the .myselector object does not revert
                        return false;
                    }
                }
            });

            // save the element in the coord itself
            coord.el = $el;

            return $el;
        },

        hasCoordinate: function (coord) {
            var self = this;
            var peg = self.getPeg(coord.x, coord.y);
            return !!peg;
        },

        getAvailableJumps: function (coord) {
            var self = this;
            var jumps = [];
            for (var i = 0; i < xs.length; i++) {
                var jumped = { x: coord.boardX + (xs[i] * 1), y: coord.boardY + (ys[i] * 1) };
                var target = { x: coord.boardX + (xs[i] * 2), y: coord.boardY + (ys[i] * 2) };
                //console.log('jumped:', jumped, 'target:', target);
                
                if (self.hasCoordinate(jumped) &&
                    self.hasCoordinate(target) &&
                    self.getPeg(jumped.x, jumped.y).hasPeg == true &&
                    self.getPeg(target.x, target.y).hasPeg == false)
                    jumps.push({
                        current: coord,
                        jumped: jumped,
                        target: target
                    });
            }

            return jumps;
        },

        isGameOver: function () {
            var self = this;
            var coordsWithPegs = $.grep(coords, function (coord, i) { return coord.hasPeg; });
                        
            for (var i = 0; i < coordsWithPegs.length; i++) {
                var coord = coordsWithPegs[i];
                var jumps = self.getAvailableJumps(coord);
                if (jumps.length > 0) {                    
                    return false;
                }
            }
            return true;
        },

        resetCoords: function () {
            var self = this;

            // set all to true first
            for (var i = 0; i < coords.length; i++) {
                coords[i].hasPeg = true;
            }

            // set random index as empty
            var index = Math.floor((Math.random() * coords.length) + 1);
            coords[index].hasPeg = false;
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

pegBoardApp.controller('GameController', function ($scope, $http, $location, userStore, timerService, boardService) {
    // make sure we've gone through the register step
    var user = userStore.get();
    if (!user.registered) {
        $location.path('/');
        return;
    }

    $scope.timerService = timerService;
    $scope.boardService = boardService;
    $scope.playing = false;    
    $scope.paused = false;
    $scope.isGameOver = false;    
    $scope.pegCount = boardService.defaultCount;    

    // start the game!
    $scope.start = function () {                
        $scope.paused = false;
        $scope.playing = true;
        $scope.isGameOver = false;
        
        // start the timer
        timerService.start();
    };

    // pause the game
    $scope.pause = function () {        
        $scope.paused = true;        
        timerService.pause();
    };

    // stop the game
    $scope.stop = function() {
        $scope.playing = false;
        $scope.paused = false;        
        $scope.pegCount = boardService.defaultCount;

        // stop the timer
        timerService.stop();
    };

    // restart the game
    $scope.restart = function() {
        $scope.stop();

        // reset the board
        boardService.resetCoords();
        boardService.placePegs();
    };

    // watch peg count    
    $scope.$watch('boardService.getCount()', function (newVal) {        
        $scope.pegCount = newVal;
    });

    // watch the time ticks
    $scope.$watch('timerService.ellapsedTime', function (newVal) {        
        $scope.ellapsedTime = newVal;
    });

    // watch for game over
    $scope.$watch('boardService.isGameOver()', function (newVal) {
        if ($scope.playing) {
            $scope.finalPegCount = $scope.pegCount;
            $scope.finalEllapsedTime = $scope.ellapsedTime;
            $scope.stop();
            $scope.isGameOver = true;
        }
    });

    // place pegs on the board    
    boardService.resetCoords();
    boardService.placePegs();
});