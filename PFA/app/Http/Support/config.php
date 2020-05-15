<?php

function config($service)
{
    $dotenv = new Dotenv\Dotenv(__DIR__.'/../../..');
    $dotenv->load();

    return [
        'dbname' => getenv('DB_DATABASE', 'homestead'),
        'host' => getenv('DB_HOST', 'localhost'),
        'port' => '3306',
        'user' => getenv('DB_USERNAME', 'homestead'),
        'password' => getenv('DB_PASSWORD', 'homestead'),
        'charset' => 'utf8'
    ];
}
