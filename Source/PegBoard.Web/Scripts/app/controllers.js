'use strict';

angular.module('pegBoardApp.controllers', [])
    .controller('NavController', function($scope, $location) {
        $scope.isActive = function(viewLocation) {
            return viewLocation === $location.path();
        };
    })
    .controller('RegisterController', function ($scope, $location, userStore) {
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
    })
    .controller('GameController', function ($scope, $http, $location, userStore, timerService, boardService) {
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
        $scope.stop = function () {
            $scope.playing = false;
            $scope.paused = false;
            $scope.pegCount = boardService.defaultCount;

            // stop the timer
            timerService.stop();
        };

        // restart the game
        $scope.restart = function () {
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