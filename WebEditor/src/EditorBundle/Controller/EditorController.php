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
        return $this->render('EditorBundle:Default:index.html.twig', array('components' => $components));
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

            // $zcel = $serializer->deserialize($bloc->getComponents(),'\EditorBundle\Entity\Bloc','json');

            $components = array();
            foreach (json_decode(json_encode($bloc->unserializeComponents()), true) as $component) {
                $imagePath = file_get_contents($this->get('kernel')->getRootDir() . '/../web' . $component["texture"]);
                $base64 = base64_encode($imagePath);

                $c = array('id' => $component["id"],
                 'name' => $component["name"],
                 'texture' => $base64,
                 'physics' => $component["physics"]);

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
     * image : File image
     * name : String
     * physics : Array
    */
    public function saveComponentAction(Request $request) {

        $em = $this->get('doctrine.orm.entity_manager');
        $json_data = $request->request->get('json_data');

        if (!is_null($json_data)) {
            //
            file_put_contents($file, $current);

            $component = new Component;
            $component->setName($json_data->getName());
            $component->setTexturePath($json_data->getTexturePath());
            $component->setPhysics($json_data->getPhysics());

            $em->persist($component);
            $em->flush();

            $a = array('status' => 'ok', 'message' => 'Poireau');
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
