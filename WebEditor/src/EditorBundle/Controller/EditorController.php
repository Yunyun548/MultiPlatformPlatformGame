<?php

namespace EditorBundle\Controller;

use Symfony\Bundle\FrameworkBundle\Controller\Controller;

class EditorController extends Controller
{
    public function indexAction()
    {
        return $this->render('EditorBundle:Default:index.html.twig');
    }
}
