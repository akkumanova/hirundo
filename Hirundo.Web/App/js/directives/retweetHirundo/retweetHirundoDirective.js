/*global angular*/
(function (angular) {
  'use strict';

  function RetweetHirundoDirective($modal, Comment) {
    var RetweetHirundoLink = function ($scope, element, attrs) {
      var RetweetModalCtrl = function ($scope, $modalInstance, commentData) {
        $scope.commentData = commentData;

        $scope.cancel = function () {
          $modalInstance.close(false);
        };

        $scope.ok = function () {
          $modalInstance.close(true);
        };
      };

      element.bind('click', function (event) {
        var comment = $scope.$eval(attrs.hdRetweetHirundo);

        var modalInstance = $modal.open({
          templateUrl: 'directives/retweetHirundo/retweetModal.html',
          controller: RetweetModalCtrl,
          windowClass: 'comment-modal',
          resolve: {
            commentData: function () {
              return {
                author: comment.author,
                authorImg: comment.authorImg,
                publishDate: comment.publishDate,
                content: comment.content
              };
            }
          }
        });

        modalInstance.result.then(function (result) {
          if (result) {
            Comment.retweet.save({ commentId: $scope.$eval(attrs.hdRetweetHirundo).commentId })
                  .$promise.then(function (commentDetails) {
              comment.isRetweeted = true;
              comment.retweets = commentDetails.retweets;
              comment.favorites = commentDetails.favorites;
            });
          }
        });

        event.stopPropagation();
      });
    };

    return {
      restrict: 'A',
      compile: function compile(tElement) {
        tElement.attr('style', 'cursor: pointer;');

        return RetweetHirundoLink;
      }
    };
  }

  RetweetHirundoDirective.$inject = [
    '$modal',
    'Comment'
  ];

  angular.module('directives').directive('hdRetweetHirundo', RetweetHirundoDirective);
}(angular));