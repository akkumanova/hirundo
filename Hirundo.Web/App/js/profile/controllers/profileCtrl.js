/*global angular*/
(function (angular) {
  'use strict';

  function ProfileCtrl($scope, $window, $state, $stateParams, User) {
    $scope.loaded = false;
    $scope.currentUserId = $window.user.userId;

    User.userData.get({ userId: $stateParams.id }).$promise.then(function (user) {
      $scope.loaded = true;
      $scope.user = user;
    });

    $scope.$on('$stateChangeSuccess', function (event, toState) {
      angular.forEach($scope.menu, function (item) {
        item.isActive = false;

        if (item.state === toState.name) {
          item.isActive = true;
        }
      });
    });

    $scope.follow = function () {
      User.userFollowing.save({ userId: $stateParams.id }).$promise.then(function () {
        $state.go($state.$current, {}, { reload: true });
      });
    };

    $scope.unfollow = function () {
      User.userFollowing.remove({ userId: $stateParams.id }).$promise.then(function () {
        $state.go($state.$current, {}, { reload: true });
      });
    };
  }

  ProfileCtrl.$inject = ['$scope', '$window', '$state', '$stateParams', 'User'];

  angular.module('profile').controller('profile.ProfileCtrl', ProfileCtrl);
}(angular));
