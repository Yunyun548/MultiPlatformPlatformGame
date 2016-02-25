<?php

namespace EditorBundle\Controller;

use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Serializer\Serializer;

class EditorController extends Controller
{
    public function indexAction()
    {
        return $this->render('EditorBundle:Default:index.html.twig');
    }

    public function persistJsonAction()
    {
        $em = $this->get('doctrine.orm.entity_manager');
        $json_data = $request->request->get('json_data');

        if (!is_null($json_data)) {

            $bloc = $serializer->deserialize($json_data,'\EditorBundle\Entity\Block','json');

            $em->persist($bloc);
            $em->flush();

            $a = array('status' => 'ok', 'message' => 'Le poireau');
            $json = json_encode($a);
            return new JsonResponse($json);
        }

        else {
            $a = array('status' => 'error', 'message' => 'Bad Parameters');
            $json = json_encode($a);
            return new JsonResponse($json);
        }
    }
}
