var app = angular.module("chatApp", []);

app.controller("chatController", ["$scope", function($scope) {
    $scope.messages = [];
    var chat = $.connection.chatHub;

    chat.client.broadcast = function (response) {
        $scope.$apply(function() {
            $scope.messages.push(response);
        });
    };

    $.connection.hub.start();

    $scope.sendMessage = function(name, message) {
        var data = { Name: name, Message: message };
        chat.server.sendMessage(data);
        $scope.messageToSend = null;
    };
}]);