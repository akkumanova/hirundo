/*global angular*/
(function (angular) {
  'use strict';

  function DetailsCtrl($scope, $window, User) {
    $scope.loaded = false;

    User.userData.get({ userId: $window.user.userId }).$promise.then(function (user) {
      $scope.loaded = true;

      $scope.user = user;
    });
  }

  DetailsCtrl.$inject = ['$scope', '$window', 'User'];

  angular.module('details').controller('details.DetailsCtrl', DetailsCtrl);
}(angular));
