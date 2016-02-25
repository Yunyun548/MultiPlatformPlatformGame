<?php

namespace EditorBundle\Controller;

use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Serializer\Serializer;
use EditorBundle\Entity\Bloc;
use EditorBundle\Entity\Component;


class EditorController extends Controller
{
    public function indexAction()
    {

        $em = $this->get('doctrine.orm.entity_manager');

        $components = $em->getRepository('EditorBundle:Component')->findAll();

        return $this->render('EditorBundle:Default:index.html.twig', array('components' => $components));
    }

    public function persistJsonAction()
    {
        $em = $this->get('doctrine.orm.entity_manager');
        $json_data = $request->request->get('json_data');

        if (!is_null($json_data)) {

            $bloc = $serializer->deserialize($json_data,'\EditorBundle\Entity\Bloc','json');

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
