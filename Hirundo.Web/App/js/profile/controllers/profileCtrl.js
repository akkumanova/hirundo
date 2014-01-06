/*global angular*/
(function (angular) {
  'use strict';

  function ProfileCtrl($scope, $window, $state, $stateParams, User) {
    $scope.loaded = false;
    $scope.currentUserId = $window.user.userId;
    $scope.menu = [
      { name: 'Hirundos', state: 'profile.hirundos', isActive: false },
      { name: 'Following', isActive: false },
      { name: 'Followers', isActive: false },
      { name: 'Favorites', isActive: false }
    ];

    User.userData.get({ userId: $stateParams.id }).$promise.then(function (user) {
      $scope.loaded = true;
      $scope.user = user;
    });

    $scope.activate = function (item) {
      angular.forEach($scope.menu, function (item) {
        item.isActive = false;
      });

      item.isActive = true;
      $state.go(item.state);
    };

    $scope.$on('$stateChangeSuccess', function (event, toState) {
      angular.forEach($scope.menu, function (item) {
        item.isActive = false;

        if (item.state === toState.name) {
          item.isActive = true;
        }
      });
    });
  }

  ProfileCtrl.$inject = ['$scope', '$window', '$state', '$stateParams', 'User'];

  angular.module('profile').controller('profile.ProfileCtrl', ProfileCtrl);
}(angular));
