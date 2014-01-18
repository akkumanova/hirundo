/*global angular*/
(function (angular) {
  'use strict';

  function ProfileCtrl($scope, $state, $navigation, User) {
    var oldUser;

    $navigation.loading = true;

    User.userProfile.get().$promise.then(function (user) {
      $navigation.loading = false;
      oldUser = {
        userId: user.userId,
        fullname: user.fullname,
        location: user.location,
        website: user.website,
        bio: user.bio
      };

      $scope.user = user;
    });

    $scope.disableSave = function () {
      return $navigation.loading ||
            $scope.user.bio && $scope.user.bio.length > 160 ||
            !$scope.image &&
            oldUser.fullname === $scope.user.fullname &&
            oldUser.location === $scope.user.location &&
            oldUser.website === $scope.user.website &&
            oldUser.bio === $scope.user.bio;
    };

    $scope.save = function () {
      $scope.user.image = $scope.image ? $scope.image.dataURL : null;

      $scope.user.$save().then(function () {
        $state.go('home');
      });
    };
  }

  ProfileCtrl.$inject = ['$scope', '$state', 'navigation.NavigationConfig', 'User'];

  angular.module('settings').controller('settings.ProfileCtrl', ProfileCtrl);
}(angular));
