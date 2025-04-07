
import 'package:flutter/material.dart';

class HighSchoolPage extends StatefulWidget {
  const HighSchoolPage({super.key});

  @override
  State<HighSchoolPage> createState() => _HighSchoolPageState();
}

class _HighSchoolPageState extends State<HighSchoolPage> { //Initial page for user. Switches to next page
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('High School Page', style: TextStyle(fontSize: 30, color: Colors.white)),
        centerTitle: true,
        backgroundColor: Colors.blue,
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: <Widget>[
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.blue[200],
            child: Text('You have likely heard of what a Power Outage is.', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.blue[200],
            child: Text('A power outage is a situation where the supply of electricity to a building, area, or entire region is temporarily stopped. ', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.blue[200],
            child: Text('This lesson will make you a power outage expert. You will be popular at the parties!', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.blue[200],
            child: Text('This lesson begins with an outage scenario of an unknown length.', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.blue[200],
            child: Text('Press the button below to continue!', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          )
        ],
      ),
      floatingActionButton: FloatingActionButton.extended(
        label: const Text('Continue!'),
        icon: const Icon(Icons.arrow_right),
        backgroundColor: Colors.green[300],
        splashColor: Colors.green,
        onPressed: () {
          Navigator.push(
            context,
            MaterialPageRoute(builder: (context) => const OutageScenario()),
          );
        },
      )
    );
  }
}

class OutageScenario extends StatefulWidget { //Where outage scenario will occur.
  const OutageScenario({super.key});

  @override
  State<OutageScenario> createState() => _OutageScenarioState();
}

class _OutageScenarioState extends State<OutageScenario> {
  //Variables to switch through as a button is pressed
  List<String> lessonsText = ['Outages can happen anywhere, at any time.',
    'When they occur, they impact devices, your environment, and more.',
    'Today, we will begin at your school. Here, an outage of 1 hour occurs.',
    'Click on objects in the image to see how they are affected. When you are done, press the Continue button!',
    'A computer can experience a surge. Along with losing unsaved data, internal components may be damaged. Click the info button for more details!',
    'An electrically powered clock may become un-synced during a surge.',
  ];
  int lessonNum = 0;
  String buttonText = 'Next';
  Color? buttonColor = Colors.grey[300];
  Color? infoButton = Colors.grey[100];
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Outage Scenario', style: TextStyle(fontSize: 30, color: Colors.black)),
        centerTitle: true,
        backgroundColor: Colors.white,
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: <Widget>[
          Stack( //Classroom image and overlaying buttons
            children: <Widget>[
               Center(
                 child: Image(
                   image: AssetImage('assets/cartoon_classroom.PNG'),
                   height: 500,
                   width: 500,
                 )
               ),
              Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 300, 10, 10),
                width: 75,
                height: 75,
                child: FloatingActionButton.extended(
                    label: const Icon(Icons.computer),
                    onPressed: () {
                      setState(() {
                        if(lessonNum < 3){
                        }else {
                          lessonNum = 4;
                          infoButton = Colors.blue[200];
                        }
                      });
                    }
                ),
              ),
              Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(350, 100, 10, 10),
                width: 75,
                height: 75,
                child: FloatingActionButton.extended(
                    label: const Icon(Icons.access_time),
                    onPressed: () {
                      setState(() {
                        if(lessonNum < 3){
                        }else {
                          lessonNum = 5;
                          infoButton = Colors.grey[100];
                        }
                      });
                    }
                ),
              ),
              Container( //more information button
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(350, 400, 10, 10),
                child: FloatingActionButton.extended(
                  label: const Icon(Icons.info_outline, color: Colors.black, size: 40),
                  backgroundColor: infoButton,
                  onPressed: () {
                    setState(() { //setState notifies framework that value has changed
                      if(lessonNum == 4){
                        Navigator.push(
                          context,
                          MaterialPageRoute(builder: (context) => const ComputerEffects()),
                        );

                      }else {
                        //here, the button should go to a page specific to each button
                        //most optimal page navigation is probably to have a single page, but easiest to implement is page/object
                      }
                    });
                  },
                ),
              ),
            ],
          ),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            width: 400,
            height: 200,
            child: Text(lessonsText[lessonNum], style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            child: FloatingActionButton.extended(
              label: Text(buttonText, style: TextStyle(fontSize: 20, color: Colors.black)),
              backgroundColor: buttonColor,
              onPressed: () {
                setState(() { //setState notifies framework that value has changed
                  if(lessonNum == 2){
                    lessonNum++;
                    buttonColor = Colors.green[300];
                    buttonText = 'Continue!';
                  } else if (lessonNum >= 3){
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => const OutageCauses()),
                    );
                  } else {
                    lessonNum++;
                  }
                });
              },
            ),
          ),
        ],
      ),
    );
  }
}

class OutageCauses extends StatefulWidget { //Page to teach user on causes of power outages
  const OutageCauses({super.key});

  @override
  State<OutageCauses> createState() => _OutageCausesState();
}

class _OutageCausesState extends State<OutageCauses> {

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text('Causes of Outages', style: TextStyle(fontSize: 30, color: Colors.black)),
          centerTitle: true,
          backgroundColor: Colors.white,
        ),
        body: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
              child: Text('Outage Causes subsection.', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
            ),
          ],
        ),
    );
  }
}

class MitigationMeasures extends StatefulWidget {
  const MitigationMeasures({super.key});

  @override
  State<MitigationMeasures> createState() => _MitigationMeasuresState();
}

class _MitigationMeasuresState extends State<MitigationMeasures> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Mitigation Measures', style: TextStyle(fontSize: 30, color: Colors.black)),
        centerTitle: true,
        backgroundColor: Colors.white,
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: <Widget>[
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            child: Text('Mitigation Measures subsection.', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
        ],
      ),
    );
  }
}


//Accessory/Extra information, beyond the outline
class ComputerEffects extends StatelessWidget {
  const ComputerEffects({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Effects on a Computer', style: TextStyle(fontSize: 30, color: Colors.black)),
        centerTitle: true,
        backgroundColor: Colors.white,
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: <Widget>[
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            child: Text('Info on how a computer is affected here, in detail.', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
        ],
      ),
    );
  }
}
