/*global angular*/
(function (angular) {
  'use strict';

  function ProfileSummaryDirective($modal, $q, $window, $navigation, User) {
    var ProfileSummaryLink = function ($scope, element, attrs) {
      var userId = attrs.hdProfileSummary;

      var UserModalCtrl = function ($scope, $state, $modalInstance, user, currentUserId, comments) {
        $scope.user = user;
        $scope.comments = comments;
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

        $q.all({
          user: User.userData.get({ userId: id }).$promise,
          comments: User.userComments.query({ userId: id, take: 2 }).$promise
        }).then(function (res) {
          $navigation.loading = false;

          $modal.open({
            templateUrl: 'directives/profileSummary/userModal.html',
            controller: UserModalCtrl,
            windowClass: 'user-modal',
            resolve: {
              user: function () {
                return res.user;
              },
              currentUserId: function () {
                return $window.user.userId;
              },
              comments: function () {
                return res.comments;
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
    '$q',
    '$window',
    'navigation.NavigationConfig',
    'User'
  ];

  angular.module('directives').directive('hdProfileSummary', ProfileSummaryDirective);
}(angular));