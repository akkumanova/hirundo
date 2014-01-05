/*global angular*/
(function (angular) {
  'use strict';

  function ProfileCtrl($scope, $window, $stateParams, User) {
    $scope.loaded = false;
    $scope.currentUserId = $window.user.userId;

    User.userData.get({ userId: $stateParams.id }).$promise.then(function (user) {
      $scope.loaded = true;
      $scope.user = user;
    });
  }

  ProfileCtrl.$inject = ['$scope', '$window', '$stateParams', 'User'];

  angular.module('profile').controller('profile.ProfileCtrl', ProfileCtrl);
}(angular));
