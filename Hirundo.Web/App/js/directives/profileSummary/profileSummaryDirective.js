﻿/*global angular*/
(function (angular) {
  'use strict';

  function ProfileSummaryDirective($modal, $window, $navigation, User) {
    var ProfileSummaryLink = function ($scope, element, attrs) {
      var userId = attrs.hdProfileSummary;

      var UserModalCtrl = function ($scope, $state, $modalInstance, user, currentUserId) {
        $scope.user = user;
        $scope.currentUserId = currentUserId;

        $scope.close = function () {
          $modalInstance.dismiss('cancel');
        };

        $scope.follow = function () {
          User.userFollowing.save({ userId: $scope.user.userId }).$promise.then(function () {
            User.userData.get({ userId: $scope.user.userId }).$promise.then(function (user) {
              $scope.user = user;
            });

            $state.go($state.$current, {}, { reload: true });
          });
        };

        $scope.unfollow = function () {
          User.userFollowing.remove({ userId: $scope.user.userId }).$promise.then(function () {
            User.userData.get({ userId: $scope.user.userId }).$promise.then(function (user) {
              $scope.user = user;
            });

            $state.go($state.$current, {}, { reload: true });
          });
        };
      };

      element.bind('click', function (event) {
        var id = $scope.$eval(userId);
        $navigation.loading = true;

        User.userData.get({ userId: id }).$promise.then(function (user) {
          $navigation.loading = false;

          $modal.open({
            templateUrl: 'directives/profileSummary/userModal.html',
            controller: UserModalCtrl,
            windowClass: 'user-modal',
            resolve: {
              user: function () {
                return user;
              },
              currentUserId: function () {
                return $window.user.userId;
              }
            }
          });
        });

        event.stopPropagation();
      });
    };

    return {
      restrict: 'A',
      compile: function compile(tElement) {
        tElement.attr('style', 'cursor: pointer;');

        return ProfileSummaryLink;
      }
    };
  }

  ProfileSummaryDirective.$inject = [
    '$modal',
    '$window',
    'navigation.NavigationConfig',
    'User'
  ];

  angular.module('directives').directive('hdProfileSummary', ProfileSummaryDirective);
}(angular));