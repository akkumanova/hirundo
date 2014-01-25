/*global angular*/
(function (angular) {
  'use strict';

  var NewHirundoModalCtrl = function ($scope, $modalInstance, Comment, userId) {
    $scope.sendHirundo = function (hirundo) {
      var newComment = {
        'Author': userId,
        'Content': hirundo.content,
        'Location': hirundo.location,
        'Image' : hirundo.image,
        'PublishDate': new Date()
      };

      return Comment.comment.save(newComment).$promise.then(function () {
        $modalInstance.close(true);
      });
    };

    $scope.close = function () {
      $modalInstance.close(false);
    };
  };

  function NavbarCtrl($scope, $state, $window, $element, $modal, navigationConfig, User, Comment) {
    function mapItems(items) {
      return items.map(function (item) {
        var newItem = {
          active: false,
          text: item.text,
          icon: item.icon,
          newTab: item.newTab,
          items: item.items ? mapItems(item.items) : []
        };

        if (item.state) {
          newItem.url = $state.href(item.state, item.stateParams);
        } else {
          newItem.url = item.url;
        }

        return newItem;
      });
    }

    $scope.$watch(
      function () {
        return navigationConfig.loading;
      },
      function (value) {
        $scope.loading = value;
      }
    );

    $scope.items = mapItems(navigationConfig.items);
    $scope.loading = navigationConfig.loading;
    $scope.username = $window.user.username;

    $scope.newHirundo = function () {
      var modalInstance = $modal.open({
        templateUrl: 'navigation/templates/newHirundo.html',
        controller: NewHirundoModalCtrl,
        windowClass: 'comment-modal',
        resolve: {
          Comment: function () {
            return Comment;
          },
          userId: function () {
            return $window.user.userId;
          }
        }
      });

      modalInstance.result.then(function (result) {
        if (result) {
          $state.go($state.$current, {}, { reload: true });
        }
      });
    };

    $scope.getUsers = function (username) {
      return User.userData.query({ username: username }).$promise.then(function (users) {
        return users;
      });
    };

    $scope.viewUser = function (user) {
      $state.go('profile', { id: user.userId });
    };

    $scope.search = function () {
      $element.find('input').triggerHandler('blur');
      $state.go('search', { username: $scope.user.username || $scope.user });
    };
  }

  NavbarCtrl.$inject = [
    '$scope',
    '$state',
    '$window',
    '$element',
    '$modal',
    'navigation.NavigationConfig',
    'User',
    'Comment'
  ];

  angular.module('navigation').controller('navigation.NavbarCtrl', NavbarCtrl);
}(angular));
