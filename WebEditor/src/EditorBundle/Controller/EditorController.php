<?php

namespace EditorBundle\Controller;

use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Serializer\Serializer;
use Symfony\Component\Serializer\Encoder\JsonEncoder;
use Symfony\Component\Serializer\Normalizer\ObjectNormalizer;
use EditorBundle\Entity\Bloc;
use EditorBundle\Entity\Component;


class EditorController extends Controller
{
    public function indexAction()
    {
        $em = $this->get('doctrine.orm.entity_manager');

        $components = $em->getRepository('EditorBundle:Component')->findAll();

        $jsonArray = array();
        foreach ($components as $component) {
            $c = array('id' => $component->getId(),
             'name' => $component->getName(),
             'texture' => $component->getTexturePath(),
             'physics' => $component->getPhysics());

            array_push($jsonArray, $c);
        }

        $j = json_encode($jsonArray);

        return $this->render('EditorBundle:Default:index.html.twig', array('components' => $components, 'jsonArray' => $j));
    }

    public function persistJsonAction(Request $request)
    {
        $em = $this->get('doctrine.orm.entity_manager');
        $json_data = $request->request->get('json_data');

        if (!is_null($json_data)) {

            $encoders = array(new JsonEncoder());
            $normalizers = array(new ObjectNormalizer());
            $serializer = new Serializer($normalizers, $encoders);

            $bloc = $serializer->deserialize($json_data,'\EditorBundle\Entity\Bloc','json');

            $em->persist($bloc);
            $em->flush();

            $components = array();
            foreach ($bloc->getComponents() as $component) {
                $imagePath = file_get_contents($component->getTexturePath());
                $base64 = base64_encode($imagePath);

                $c = array('id' => $component->getId(),
                 'name' => $component->getName(),
                 'texture' => $base64,
                 'physics' => $component->getPhysics());

                array_push($components, $c);
            }

            $blocJson = array('id' => $bloc->getId(),
             'name' => $bloc->getName(),
             'components' => $components);

            $a = array('status' => 'ok', 'bloc' => $blocJson);
            $json = json_encode($a);
            return new JsonResponse($json);
        }

        else {
            $a = array('status' => 'error', 'message' => 'Bad Parameters');
            $json = json_encode($a);
            return new JsonResponse($json);
        }
    }

    /* Save a new Component
     * Parameters
     *
     *
     *
    */
    public function saveComponentAction(Request $request) {

        $em = $this->get('doctrine.orm.entity_manager');
        $json_data = $request->request->get('json_data');

        if (!is_null($json_data)) {
            //
            file_put_contents($file, $current);
        }

        else {
            $a = array('status' => 'error', 'message' => 'Bad Parameters');
            $json = json_encode($a);
            return new JsonResponse($json);
        }

    }
}
