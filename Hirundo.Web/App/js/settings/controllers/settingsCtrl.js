/*global angular*/
(function (angular) {
  'use strict';

  function SettingsCtrl($scope, $window) {
    $scope.user = $window.user;
  }

  SettingsCtrl.$inject = ['$scope', '$window'];

  angular.module('settings').controller('settings.SettingsCtrl', SettingsCtrl);
}(angular));
