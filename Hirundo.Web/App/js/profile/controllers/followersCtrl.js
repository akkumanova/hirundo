/*global angular*/
(function (angular) {
  'use strict';

  function FollowersCtrl($scope, $stateParams) {
    $scope.userId = $stateParams.id;
  }

  FollowersCtrl.$inject = ['$scope', '$stateParams'];

  angular.module('profile').controller('profile.FollowersCtrl', FollowersCtrl);
}(angular));
