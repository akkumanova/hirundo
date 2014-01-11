/*global angular*/
(function (angular) {
  'use strict';

  function FollowingCtrl($scope, $stateParams) {
    $scope.userId = $stateParams.id;
  }

  FollowingCtrl.$inject = ['$scope', '$stateParams'];

  angular.module('profile').controller('profile.FollowingCtrl', FollowingCtrl);
}(angular));
