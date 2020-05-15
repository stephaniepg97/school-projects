<?php

function flash($type, $message = null)
{
    $key = "flash.$type";
    if (!is_null($message)) {
        $_SESSION[$key] = $message;
        return;
    }

    if (!isset($_SESSION[$key])) {
        return;
    }

    register_shutdown_function(function () use ($key) {
        unset($_SESSION[$key]);
    });

    return $_SESSION[$key];
}
