/*global angular*/
(function (angular) {
  'use strict';

  function ProfileSummaryDirective($modal, $window, User) {
    var ProfileSummaryLink = function ($scope, element, attrs) {
      var userId = attrs.hdProfileSummary;

      var UserModalCtrl = function ($scope, $modalInstance, user, currentUserId) {
        $scope.user = user;
        $scope.currentUserId = currentUserId;

        $scope.close = function () {
          $modalInstance.dismiss('cancel');
        };
      };

      element.bind('click', function (event) {
        var id = $scope.$eval(userId);

        User.userData.get({ userId: id }).$promise.then(function (user) {
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

  ProfileSummaryDirective.$inject = ['$modal', '$window', 'User'];

  angular.module('directives').directive('hdProfileSummary', ProfileSummaryDirective);
}(angular));