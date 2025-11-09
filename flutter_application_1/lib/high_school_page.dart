
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class HighSchoolPage extends StatefulWidget {
  const HighSchoolPage({super.key});

  @override
  State<HighSchoolPage> createState() => _HighSchoolPageState();
}

class _HighSchoolPageState extends State<HighSchoolPage> { //Initial page for user. Switches to next page.
  // Insert learning outcomes**
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Introduction', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,

          children: <Widget>[
            Flexible(
              flex: 4,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 10, 10, 0),
                color: Colors.grey[300],
                child: Text('You have likely heard of what a Power Outage is.', style: TextStyle(fontSize: 18)),
              ),
            ),
            Spacer(),
            Flexible(
              flex: 4,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                color: Colors.grey[300],
                child: Text('A power outage is a situation where the supply of electricity to a building, area, or entire region is temporarily stopped. ', style: TextStyle(fontSize: 18)),
              ),
            ),
            Spacer(),
            Flexible(
              flex: 4,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                color: Colors.grey[300],
                child: Text('The following lesson will make you a power outage expert. You will be popular at the parties!',  style: TextStyle(fontSize: 18)),
              ),
            ),
            Spacer(),
            Flexible(
              flex: 4,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                color: Colors.grey[300],
                child: Text('This lesson begins with an outage scenario of an unknown length. Press the button below to continue!',  style: TextStyle(fontSize: 18)),
              ),
            ),
            Spacer(),
            Flexible(
              flex: 4,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                child: FloatingActionButton.extended(
                  label: Text('Continue!', style: TextStyle(fontSize: 20, color: Colors.black)),
                  icon: const Icon(Icons.arrow_right),
                  backgroundColor: Colors.green[300],
                  splashColor: Colors.green,
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => const OutageScenario()),
                    );
                  },
                ),
              ),
            ),
            Spacer(),
          ],
        ),
      ),
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
  List<String> lessonsText = ['Outages can happen anywhere, at any time. When they occur, they impact devices, your environment, and more. You may have even noticed some of these effects, such as when your oven resets its time following an outage.',
    'Today, we will start with an outage occurring at your school. The duration of the outage is for one hour. During this time, there will be varying impacts on devices.',
    'Click on the listed objects below to learn how each are affected. When you are done, press the Continue button!',
    'A computer can experience a surge. Along with losing unsaved data, internal components may be damaged. Click the info button for more details!',
    'An electrically powered clock may become un-synced during a surge.',
    'The AC Unit for your building will no longer be powered and will eventually increase or decrease to match outside temperature. Click the info button for more details!',

  ];
  int lessonNum = 0;
  String buttonText = 'Next';
  Color? buttonColor = Colors.grey[300];
  Color? infoButton = Colors.grey[100];
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Outage Scenario', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold))),//style: TextStyle(fontSize: 30, color: Colors.black)),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,

          children: <Widget>[
            // Flexible(
            //   flex: 8,
            //   child: Stack( //Classroom image and overlaying buttons
            //     children: <Widget>[
            //        Center(
            //          child: Image(
            //            image: AssetImage('assets/cartoon_classroom.PNG'),
            //            height: 300,
            //            width: 500,
            //          )
            //        ),
            //       Container( //computer button
            //         padding: const EdgeInsets.all(10.0),
            //         margin: const EdgeInsets.fromLTRB(10, 300, 10, 10),
            //         width: 75,
            //         height: 75,
            //         child: FloatingActionButton.extended(
            //             label: const Icon(Icons.computer),
            //             onPressed: () {
            //               setState(() {
            //                 if(lessonNum < 3){
            //                 }else {
            //                   lessonNum = 4;
            //                   infoButton = Colors.blue[200];
            //                 }
            //               });
            //             }
            //         ),
            //       ),
            //
            //     Container( //clock button
            //       padding: const EdgeInsets.all(10.0),
            //       margin: const EdgeInsets.fromLTRB(325, 100, 10, 10),
            //       width: 75,
            //       height: 75,
            //       child: FloatingActionButton.extended(
            //           label: const Icon(Icons.access_time),
            //           onPressed: () {
            //             setState(() {
            //               if(lessonNum < 3){
            //               }else {
            //                 lessonNum = 5;
            //                 infoButton = Colors.grey[100];
            //               }
            //             });
            //           }
            //       ),
            //     ),
            //     Container( //AC button
            //       padding: const EdgeInsets.all(10.0),
            //       margin: const EdgeInsets.fromLTRB(10, 100, 10, 10),
            //       width: 75,
            //       height: 75,
            //       child: FloatingActionButton.extended(
            //           label: const Icon(Icons.ac_unit),
            //           onPressed: () {
            //             setState(() {
            //               if(lessonNum < 3){
            //               }else {
            //                 lessonNum = 6;
            //                 infoButton = Colors.blue[200];
            //               }
            //             });
            //           }
            //       ),
            //     ),
            //     Container( //more information button
            //       padding: const EdgeInsets.all(10.0),
            //       margin: const EdgeInsets.fromLTRB(325, 400, 10, 10),
            //       width: 75,
            //       height: 75,
            //       child: FloatingActionButton.extended(
            //         label: const Icon(Icons.info_outline, color: Colors.black, size: 30),
            //         backgroundColor: infoButton,
            //         onPressed: () {
            //           setState(() { //setState notifies framework that value has changed
            //             if(lessonNum == 4){
            //               Navigator.push(
            //                 context,
            //                 MaterialPageRoute(builder: (context) => const ComputerEffects()),
            //               );
            //             }else if (lessonNum == 6){
            //               Navigator.push(
            //                 context,
            //                 MaterialPageRoute(builder: (context) => const ACUnit()),
            //               );
            //             }
            //             else {
            //               //here, the button should go to a page specific to each button
            //               //most optimal page navigation is probably to have a single page, but easiest to implement is page/object
            //             }
            //           });
            //         },
            //       ),
            //     ),
            //   ],
            // ),
            // ),
            Flexible(
              flex: 3,
              child: Center(
                  child: Image(
                    image: AssetImage('assets/cartoon_classroom.PNG'),
                    height: 300,
                    width: 300,
                  )
              ),
            ),
            Flexible(
              flex: 3,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 10),
                width: 400,
                height: 200,
                child: Text(lessonsText[lessonNum], style: TextStyle(fontSize: 18)),
              ),
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: <Widget>[
                Container( //more information button
                  padding: const EdgeInsets.all(10.0),
                  margin: const EdgeInsets.fromLTRB(0,0,10,0),
                  width: 75,
                  height: 75,
                  child: FloatingActionButton.extended(
                    label: const Icon(Icons.info_outline, color: Colors.black, size: 30),
                    backgroundColor: infoButton,
                    onPressed: () {
                      setState(() { //setState notifies framework that value has changed
                        if(lessonNum == 3){
                          Navigator.push(
                            context,
                            MaterialPageRoute(builder: (context) => const ComputerEffects()),
                          );
                        }else if (lessonNum == 5){
                          Navigator.push(
                            context,
                            MaterialPageRoute(builder: (context) => const ACUnit()),
                          );
                        }
                        else {
                          //here, the button should go to a page specific to each button
                          //most optimal page navigation is probably to have a single page, but easiest to implement is page/object
                        }
                      });
                    },
                  ),
                ),
              ],
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: <Widget>[
                Container(
                  padding: const EdgeInsets.all(0),
                  margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                  child: FloatingActionButton.extended(
                    label: Text('Computers', style: TextStyle(color: Colors.black)),
                    icon: Icon(Icons.computer, color: Colors.black),
                    backgroundColor: Colors.grey[400],
                      onPressed: () {
                        setState(() {
                          if(lessonNum < 2){
                          }else {
                            lessonNum = 3;
                            infoButton = Colors.green[300];
                          }
                        });
                      }
                  ),
                ),
              ],
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: <Widget>[
                Container(
                  padding: const EdgeInsets.all(10),
                  margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                  child: FloatingActionButton.extended(
                    label: Text('AC Unit', style: TextStyle(color: Colors.black)),
                    icon: Icon(Icons.ac_unit, color: Colors.black),
                    backgroundColor: Colors.grey[400],
                      onPressed: () {
                        setState(() {
                          if(lessonNum < 2){
                          }else {
                            lessonNum = 5;
                            infoButton = Colors.green[300];
                          }
                        });
                      }
                  ),
                ),
                Container(
                  padding: const EdgeInsets.all(10),
                  margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                  child: FloatingActionButton.extended(
                    label: Text('Clocks', style: TextStyle(color: Colors.black)),
                    icon: Icon(Icons.access_time, color: Colors.black),
                    backgroundColor: Colors.grey[400],
                      onPressed: () {
                        setState(() {
                          if(lessonNum < 2){
                          }else {
                            lessonNum = 4;
                            infoButton = Colors.grey[100];
                          }
                        });
                      }
                  ),
                ),
              ],
            ),

            Flexible(
              flex: 2,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
                child: FloatingActionButton.extended(
                  label: Text(buttonText, style: TextStyle(fontSize: 20, color: Colors.black)),
                  backgroundColor: buttonColor,
                  onPressed: () {
                    setState(() { //setState notifies framework that value has changed
                      if(lessonNum == 1){
                        lessonNum++;
                        buttonColor = Colors.green[300];
                        buttonText = 'Continue!';
                      } else if (lessonNum >= 2){
                        Navigator.push(
                          context,
                          MaterialPageRoute(builder: (context) => const MoreOutageImpacts()),
                        );
                      } else {
                        lessonNum++;
                      }
                    });
                  },
                ),
              ),
              ),
          ],
        ),
      )
    );
  }
}

class MoreOutageImpacts extends StatefulWidget {
  const MoreOutageImpacts({super.key});

  @override
  State<MoreOutageImpacts> createState() => _MoreOutageImpactsState();
}

class _MoreOutageImpactsState extends State<MoreOutageImpacts> {
  List<String> lessonsText = ['There are more devices that can be impacted beyond your classroom, such as in your household. Click the buttons below to learn about them!',
    'Microwaves, stove-tops, air fryers, ovens and other cooking appliances can no longer be used. This makes cooking food difficult.',
    'Water pumps will no longer function properly. Water may be contaminated, and flood could be possible due to sump pump failure. Therefore, it is highly advised to not drink water from tap during an outage.',
    'Similar to AC Units, fridges will lose internal temperature until it reaches an equilibrium temperature. Food at this temperature will spoil more quickly, so it is suggested that fridges stay closed during outages.',
    'Wifi communications will be lost without power. This is due to your router and modem being disconnected from a power source, which are both responsible for creating a signal for devices to connect the internet.',
  ];
  int lessonNum = 0;
  String photo = 'assets/cartoon_house.PNG';
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Additional Impacts', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Container(
              padding: const EdgeInsets.fromLTRB(10, 10, 10, 10),
              margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
              child: Text(lessonsText[lessonNum], style: TextStyle(fontSize: 18)),
            ),
            Center(
                child: Image(
                  image: AssetImage(photo),
                  height: 250,
                  width: 250,
                )
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: <Widget>[
                Container(
                  padding: const EdgeInsets.all(10),
                  margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                  child: FloatingActionButton.extended(
                      label: Text('Microwaves', style: TextStyle(color: Colors.black)),
                      icon: Icon(Icons.microwave, color: Colors.black),
                      backgroundColor: Colors.grey[400],
                      onPressed: () {
                        setState(() {
                          lessonNum = 1;
                          photo = 'assets/microwaveOff.PNG';
                        });
                      },
                  ),
                ),
                Container(
                  padding: const EdgeInsets.all(10),
                  margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                  child: FloatingActionButton.extended(
                    label: Text('Water Pumps', style: TextStyle(color: Colors.black)),
                    icon: Icon(Icons.water_drop, color: Colors.black),
                    backgroundColor: Colors.grey[400],
                    onPressed: () {
                      setState(() {
                        lessonNum = 2;
                        photo = 'assets/tap_glass_sink.jpg';
                      });
                    },
                  ),
                ),
              ],
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: <Widget>[
                Container(
                  padding: const EdgeInsets.all(10),
                  margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                  child: FloatingActionButton.extended(
                    label: Text('Fridges', style: TextStyle(color: Colors.black)),
                    icon: Icon(Icons.ac_unit, color: Colors.black),
                    backgroundColor: Colors.grey[400],
                    onPressed: () {
                      setState(() {
                        lessonNum = 3;
                        photo = 'assets/open_fridge.PNG';
                      });
                    },
                  ),
                ),
                Container(
                  padding: const EdgeInsets.all(10),
                  margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                  child: FloatingActionButton.extended(
                    label: Text('Wifi', style: TextStyle(color: Colors.black)),
                    icon: Icon(Icons.signal_cellular_off, color: Colors.black),
                    backgroundColor: Colors.grey[400],
                    onPressed: () {
                      setState(() {
                        lessonNum = 4;
                        photo = 'assets/internetGraphic.png';
                      });
                    },
                  ),
                ),
              ],
            ),
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
                child: FloatingActionButton.extended(
                  label: Text('Continue!', style: TextStyle(fontSize: 20, color: Colors.black)),
                  backgroundColor: Colors.green[300],
                  onPressed: () {
                    setState(() { //setState notifies framework that value has changed
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (context) => const OutageCauses()),
                      );
                    });
                  },
                ),
            ),
          ],
        ),
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
  //List of Variables
  List<String> lessonsText = ['Power outages happen for several different reasons. Depending on the cause, a power outage may last anywhere from a few seconds to several weeks.',
    'Usually your power provider, called a utility, will do their best to restore power as fast as possible but may take more time depending on the cause.',
    'These causes are listed above. Click one to learn about it!',
    'Natural disasters range from hurricanes, earthquakes, extreme heat and cold, and more. They can happen at any time. Click on the info button to learn more!',
    'Animals, such as squirrels or birds, may often use power lines to navigate areas or perch. They may cause transients on the line.',
    'Human error can cause ...',
  ];
  int lessonNum = 0;
  String buttonText = 'Next';
  Color? buttonColor = Colors.grey[300];
  Color? infoButton = Colors.grey[100];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text('Causes of Outages', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold))),
          centerTitle: true,
          backgroundColor: Color(0xFF800000),
        ),
        body: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Flexible(
              flex: 3,
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  Container(
                    padding: const EdgeInsets.all(10),
                    margin: const EdgeInsets.fromLTRB(10, 10, 5, 10),
                    child: Material(
                      color: Colors.grey[600],
                      elevation: 12,
                      borderRadius: BorderRadius.circular(20),
                      clipBehavior: Clip.antiAliasWithSaveLayer,
                      child: InkWell(
                        onTap: () {
                          setState(() {
                            if(lessonNum < 2){
                            }else {
                              lessonNum = 3;
                              infoButton = Colors.green[200];
                            }
                          });
                        },
                        child: Column(
                          children: [
                            Ink.image(
                              image: AssetImage('assets/hurricane.PNG'),
                              height: 40,
                              width: 170,
                              fit: BoxFit.cover,
                            ),
                            SizedBox(height: 6),
                            Text('Natural Disasters',style: TextStyle(fontSize: 18, color: Colors.white)),
                          ],
                        ),
                      ),
                    ),
                  ),
                  Container(
                    padding: const EdgeInsets.all(10),
                    margin: const EdgeInsets.fromLTRB(5, 10, 10, 10),
                    child: Material(
                      color: Colors.grey[600],
                      elevation: 12,
                      borderRadius: BorderRadius.circular(20),
                      clipBehavior: Clip.antiAliasWithSaveLayer,
                      child: InkWell(
                        onTap: () {
                          if (lessonNum >= 2) {
                            Navigator.push(
                              context,
                              MaterialPageRoute(builder: (context) => const Animals()),
                            );
                          }
                        },
                        child: Column(
                          children: [
                            Ink.image(
                              image: AssetImage('assets/squirrel.PNG'),
                              height: 40,
                              width: 170,
                              fit: BoxFit.cover,
                            ),
                            SizedBox(height: 6),
                            Text('Animals',style: TextStyle(fontSize: 18, color: Colors.white)),
                          ],
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
            Flexible(
              flex: 3,
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  Container(
                    padding: const EdgeInsets.all(10),
                    margin: const EdgeInsets.fromLTRB(10, 10, 5, 10),
                    child: Material(
                      color: Colors.grey[600],
                      elevation: 12,
                      borderRadius: BorderRadius.circular(20),
                      clipBehavior: Clip.antiAliasWithSaveLayer,
                      child: InkWell(
                        onTap: () {
                          if (lessonNum >= 2) {
                            Navigator.push(
                              context,
                              MaterialPageRoute(builder: (context) => const HumanError()),
                            );
                          }
                        },
                        child: Column(
                          children: [
                            Ink.image(
                              image: AssetImage('assets/fallenPole.PNG'),
                              height: 40,
                              width: 170,
                              fit: BoxFit.cover,
                            ),
                            SizedBox(height: 6),
                            Text('Human Error',style: TextStyle(fontSize: 18, color: Colors.white)),
                          ],
                        ),
                      ),
                    ),
                  ),
                  Container(
                    padding: const EdgeInsets.all(10),
                    margin: const EdgeInsets.fromLTRB(5, 10, 10, 10),
                    child: Material(
                      color: Colors.grey[600],
                      elevation: 12,
                      borderRadius: BorderRadius.circular(20),
                      clipBehavior: Clip.antiAliasWithSaveLayer,
                      child: InkWell(
                        onTap: () {
                          if (lessonNum >= 2) {
                            Navigator.push(
                              context,
                              MaterialPageRoute(builder: (context) => const EquipmentFailure()),
                            );
                          }
                        },
                        child: Column(
                          children: [
                            Ink.image(
                              image: AssetImage('assets/lineEquipment.PNG'),
                              height: 40,
                              width: 170,
                              fit: BoxFit.cover,
                            ),
                            SizedBox(height: 6),
                            Text('Equipment Failure',style: TextStyle(fontSize: 18, color: Colors.white)),
                          ],
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
            Flexible(
              flex: 3,
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  Container(
                    padding: const EdgeInsets.all(10),
                    margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                    child: Material(
                      color: Colors.grey[600],
                      elevation: 12,
                      borderRadius: BorderRadius.circular(20),
                      clipBehavior: Clip.antiAliasWithSaveLayer,
                      child: InkWell(
                        onTap: () {
                          if (lessonNum >= 2) {
                            Navigator.push(
                              context,
                              MaterialPageRoute(builder: (context) => const PlannedOutages()),
                            );
                          }
                        },
                        child: Column(
                          children: [
                            Ink.image(
                              image: AssetImage('assets/powerOutage.PNG'),
                              height: 40,
                              width: 175,
                              fit: BoxFit.cover,
                            ),
                            SizedBox(height: 6),
                            Text('Planned Outages',style: TextStyle(fontSize: 18, color: Colors.white)),
                          ],
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
            Flexible(
              flex: 3,
              child: Row(
                mainAxisAlignment: MainAxisAlignment.end,
                children: [
                  Container( //more information button; this may be removed for a sized box instead**
                    padding: const EdgeInsets.all(10.0),
                    margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                    height: 75,
                    width: 75,
                    child: FloatingActionButton.extended(
                      label: const Icon(Icons.info_outline, color: Colors.black, size: 30),
                      backgroundColor: infoButton,
                      onPressed: () {
                        setState(() { //setState notifies framework that value has changed
                          if(lessonNum == 3){
                            Navigator.push(
                              context,
                              MaterialPageRoute(builder: (context) => const NaturalDisasters()),
                            );
                          }
                          else {}
                        });
                      },
                    ),
                  ),
                ],
              ),
            ),
            Flexible(
              flex: 4,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
                width: 400,
                height: 200,
                child: Text(lessonsText[lessonNum], style: TextStyle(fontSize: 18)),
              ),
            ),
            Flexible(
              flex: 3,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
                child: FloatingActionButton.extended(
                  label: Text(buttonText, style: TextStyle(fontSize: 18, color: Colors.black)),
                  backgroundColor: buttonColor,
                  onPressed: () {
                    setState(() { //setState notifies framework that value has changed
                      if(lessonNum == 1){
                        lessonNum++;
                        buttonColor = Colors.green[300];
                        buttonText = 'Continue!';
                      } else if (lessonNum >= 2){
                        Navigator.push(
                          context,
                          MaterialPageRoute(builder: (context) => const IntroMitigationMeasures()),
                        );
                      } else {
                        lessonNum++;
                      }
                    });
                  },
                ),
              ),
            ),
          ],

        ),
    );
  }
}

class IntroMitigationMeasures extends StatefulWidget {
  const IntroMitigationMeasures({super.key});

  @override
  State<IntroMitigationMeasures> createState() => _IntroMitigationMeasuresState();
}

class _IntroMitigationMeasuresState extends State<IntroMitigationMeasures> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Mitigation Measures', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: <Widget>[
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.grey[300],
            child: Text('It is important to be prepared for outages. You should aim to have a plan when one occurs.', style: TextStyle(fontSize: 18, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.grey[300],
            child: Text('A household may have different plans based on expected length. For short-term ones, a plan might not even be needed!', style: TextStyle(fontSize: 18, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.grey[300],
            child: Text('Even so, households should be ready for the worst.', style: TextStyle(fontSize: 18, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.grey[300],
            child: Text('Here, you will learn about the many different ways to be prepared for a power outage!', style: TextStyle(fontSize: 18, color: Colors.blue[1000])),
          ),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            child: FloatingActionButton.extended(
              label: Text('Continue!', style: TextStyle(fontSize: 20, color: Colors.black)),
              backgroundColor: Colors.green[300],
              splashColor: Colors.green,
              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (context) => const OutagePreparation()),
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}

class OutagePreparation extends StatefulWidget {
  const OutagePreparation({super.key});

  @override
  State<OutagePreparation> createState() => _OutagePreparationState();
}

class _OutagePreparationState extends State<OutagePreparation> {
  List<String> lessonsText = ['Here are some important ways to be ready for a power outage. Click through each in order to learn all about them!',
    'Contact your utility. They remain active during power outages. Inform them of the situation, and learn if it is a planned outage or not. Local utilities often have a power outage tracker and provide updates.',
    'Keep a shelter in mind. You may decide to stay in your home or at an emergency shelter. Emergency shelters include mass care shelters, homes of relatives, hotels, etc.',
    'Maintain a communication plan with your guardian or family members. This includes phone calls, group chats, radios, etc.',
    'Keep and maintain an emergency kit. For materials to keep inside this emergency kit, click the info button!',
  ];
  int lessonNum = 0;
  int trackLesson = 0;
  Color? infoButton = Colors.grey[100];
  Color? initialState1 = Colors.green[100];
  Color? initialState2 = Colors.grey[100];
  Color? initialState3 = Colors.grey[100];
  Color? initialState4 = Colors.grey[100];
  Color? continueButton = Colors.grey[100];
  String continueText = 'Click the buttons above!';

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Mitigation Measures', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Flexible(
              flex: 3,
              child: Container(
                padding: const EdgeInsets.all(10),
                margin: const EdgeInsets.fromLTRB(10, 5, 10, 5),
                child: FloatingActionButton.extended(
                  label: Text('1. Get informed', style: TextStyle(fontSize: 20, color: Colors.black)),
                  backgroundColor: initialState1,
                  onPressed: () {
                    setState(() {
                      lessonNum = 1;
                      infoButton = Colors.grey[100];
                      initialState1 = Colors.green[300];
                      initialState2 = Colors.grey[100];
                      initialState3 = Colors.grey[100];
                      initialState4 = Colors.grey[100];
                      if (trackLesson == 0){
                        trackLesson += 1;
                        initialState2 = Colors.green[100];
                      }
                    });
                  },
                ),
              ),
            ),
            Flexible(
              flex: 3,
              child: Container(
                padding: const EdgeInsets.all(10),
                margin: const EdgeInsets.fromLTRB(10, 5, 10, 5),
                child: FloatingActionButton.extended(
                  label: Text('2. Have a shelter in mind', style: TextStyle(fontSize: 20, color: Colors.black)),
                  backgroundColor: initialState2,
                  onPressed: () {
                    setState(() {
                      if(trackLesson >= 1) {
                        lessonNum = 2;
                        infoButton = Colors.grey[100];
                        initialState1 = Colors.grey[100];
                        initialState2 = Colors.green[300];
                        initialState3 = Colors.grey[100];
                        initialState4 = Colors.grey[100];
                        if (trackLesson == 1){
                          trackLesson += 1;
                          initialState3 = Colors.green[100];
                        }
                      }
                      else{}
                    });
                  },
                ),
              ),
            ),
            Flexible(
              flex: 3,
              child: Container(
                padding: const EdgeInsets.all(10),
                margin: const EdgeInsets.fromLTRB(10, 5, 10, 5),
                child: FloatingActionButton.extended(
                  label: Text('3. Have plans for communication', style: TextStyle(fontSize: 20, color: Colors.black)),
                  backgroundColor: initialState3,
                  onPressed: () {
                    setState(() {
                      if(trackLesson >= 2) {
                        lessonNum = 3;
                        infoButton = Colors.grey[100];
                        initialState1 = Colors.grey[100];
                        initialState2 = Colors.grey[100];
                        initialState3 = Colors.green[300];
                        initialState4 = Colors.grey[100];
                        if (trackLesson == 2){
                          trackLesson += 1;
                          initialState4 = Colors.green[100];
                        }
                      }
                      else{}
                    });
                  },
                ),
              ),
            ),
            Flexible(
              flex: 3,
              child: Container(
                padding: const EdgeInsets.all(10),
                margin: const EdgeInsets.fromLTRB(10, 5, 10, 5),
                child: FloatingActionButton.extended(
                  label: Text('4. Keep an emergency kit', style: TextStyle(fontSize: 20, color: Colors.black)),
                  backgroundColor: initialState4,
                  onPressed: () {
                    setState(() {
                      if (trackLesson >= 3) {
                        lessonNum = 4;
                        initialState1 = Colors.grey[100];
                        initialState2 = Colors.grey[100];
                        initialState3 = Colors.grey[100];
                        initialState4 = Colors.green[300];
                        infoButton = Colors.green[100];
                        if (trackLesson == 3){
                          trackLesson += 1;
                          continueButton = Colors.green[200];
                          continueText = 'Continue!';
                        }
                      }
                      else{}
                    });
                  },
                ),
              ),
            ),
            Flexible(
              flex: 3,
              child: Row(
                mainAxisAlignment: MainAxisAlignment.end,
                children: [
                  Container( //more information button
                    padding: const EdgeInsets.all(10.0),
                    margin: const EdgeInsets.fromLTRB(0, 0, 10, 0),
                    height: 75,
                    width: 75,
                    child: FloatingActionButton.extended(
                      label: const Icon(Icons.info_outline, color: Colors.black, size: 30),
                      backgroundColor: infoButton,
                      onPressed: () {
                        setState(() { //setState notifies framework that value has changed
                          if(lessonNum == 4){
                            Navigator.push(
                              context,
                              MaterialPageRoute(builder: (context) => const EmergencyKit()),
                            );
                          }
                          else {}
                        });
                      },
                    ),
                  ),
                ],
              ),
            ),
            Flexible(
              flex: 8,
              child: Container(
                padding: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
                child: Text(lessonsText[lessonNum], style: TextStyle(fontSize: 18, color: Colors.black)),
              ),
            ),
            Flexible(
              flex: 3,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
                child: FloatingActionButton.extended(
                  label: Text(continueText, style: TextStyle(fontSize: 20, color: Colors.black)),
                  backgroundColor: continueButton,
                  onPressed: () {
                    setState(() { //setState notifies framework that value has changed
                      if(trackLesson == 4) {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                              builder: (context) => const DuringOutage()),
                        );
                      }
                    });
                  },
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
class DuringOutage extends StatefulWidget {
  const DuringOutage({super.key});

  @override
  State<DuringOutage> createState() => _DuringOutageState();
}

class _DuringOutageState extends State<DuringOutage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('During an Outage', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: <Widget>[
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
            child: Text('During an outage, be sure to keep the following in mind:', style: TextStyle(fontSize: 20)),
          ),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
            color: Colors.green[100],
            child: Text('Keep freezers and refrigerators closed to preserve internal temperature.', style: TextStyle(fontSize: 18)),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
            color: Colors.green[100],
            child: Text('In case you have a generator, use it outdoors. This is to avoid carbon monoxide poisoning.', style: TextStyle(fontSize: 18)),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
            color: Colors.green[100],
            child: Text('Disconnect electronics and appliances to avoid electrical surges after the power returns.', style: TextStyle(fontSize: 18)),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.green[100],
            child: Text('Do not use gas stoves or ovens to heat homes.', style: TextStyle(fontSize: 18)),
          ),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            child: FloatingActionButton.extended(
              label: Text('Continue!', style: TextStyle(fontSize: 20, color: Colors.black)),
              backgroundColor: Colors.green[300],
              splashColor: Colors.green,
              onPressed: () {
                Navigator.push(
                  context,
                  //MaterialPageRoute(builder: (context) => const IntroPlannedOutage()),
                  MaterialPageRoute(builder: (context) => const EndOfLesson())
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}

class IntroPlannedOutage extends StatefulWidget {
  const IntroPlannedOutage({super.key});

  @override
  State<IntroPlannedOutage> createState() => _IntroPlannedOutageState();
}

class _IntroPlannedOutageState extends State<IntroPlannedOutage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Planned Outage', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: <Widget>[
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.grey[300],
            child: Text('The next lesson will test you on your outage knowledge.', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.grey[300],
            child: Text('Imagine that it is a Saturday at 1pm. Outside, there is a storm brewing.', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.grey[300],
            child: Text('You are warned that there will be an outage happening in an hour, for 5 hours long.', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            color: Colors.grey[300],
            child: Text('The next page will be a quiz on what to do in your house. After this, you will be a certified outage expert!', style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
          SizedBox(width: 20, height: 10),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            child: FloatingActionButton.extended(
              label: Text('Continue!', style: TextStyle(fontSize: 20, color: Colors.black)),
              backgroundColor: Colors.green[300],
              splashColor: Colors.green,
              onPressed: () {
                Navigator.push(
                  context,
                   MaterialPageRoute(builder: (context) => const PlannedOutage()),
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}

class PlannedOutage extends StatefulWidget {
  const PlannedOutage({super.key});

  @override
  State<PlannedOutage> createState() => _PlannedOutageState();
}

class _PlannedOutageState extends State<PlannedOutage> {
  List<String> questionsText = ['Question #1: .............',
    'Question #2: .........................',
    'Question #3: .........................',
    'Question #4: ..........................',
    'Communications, such as internet, will be lost due to ...',
  ];
  List<String> questionOne = ['A: Incorrect Answer 1',
    'B: Incorrect Answer 2',
    'C: Correct Answer', //Correct
    'D: Incorrect Answer 3',
    'Incorrect Answer 1 Response:',
    'Incorrect Answer 2 Response:',
    'Incorrect Answer 3 Response:',
    'Correct Answer Response:',
  ];
  List<String> questionTwo = ['A: Incorrect Answer 1',
    'B: Correct Answer', //Correct
    'C: Incorrect Answer 2',
    'D: Incorrect Answer 3',
    'Incorrect Answer 1 Response:',
    'Incorrect Answer 2 Response:',
    'Incorrect Answer 3 Response:',
    'Correct Answer Response:',
  ];
  List<String> questionThree = ['A: Correct Answer', //Correct
    'B: Incorrect Answer 1',
    'C: Incorrect Answer 2',
    'D: Incorrect Answer 3',
    'Incorrect Answer 1 Response:',
    'Incorrect Answer 2 Response:',
    'Incorrect Answer 3 Response:',
    'Correct Answer Response:',
  ];
  List<String> questionFour = ['A: Incorrect Answer 1',
    'B: Incorrect Answer 2',
    'C: Incorrect Answer 3',
    'D: Correct Answer', //Correct
    'Incorrect Answer 1 Response:',
    'Incorrect Answer 2 Response:',
    'Incorrect Answer 3 Response:',
    'Correct Answer Response:',
  ];


  int questionNum = 0;
  int correctAns = 0;
  String photo = 'assets/placeholder.jpg';
  String response = 'This is where response text will go.';
  String boxA = 'A: Incorrect Answer 1';
  String boxB = 'B: Incorrect Answer 2';
  String boxC = 'C: Correct Answer';
  String boxD = 'D: Incorrect Answer 3';
  String buttonText = 'Next';
  Color? nextButton = Colors.grey[300];
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Planned Outage Quiz', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: <Widget>[
          Center(
              child: Image(
                image: AssetImage(photo),
                height: 300,
                width: 300,
              )
          ),
          Container(
            padding: const EdgeInsets.fromLTRB(10, 10, 10, 10),
            margin: const EdgeInsets.fromLTRB(10, 10, 10, 10),
            child: Text(questionsText[questionNum], style: TextStyle(fontSize: 25, color: Colors.blue[1000])),
          ),
          Container(
            padding: const EdgeInsets.fromLTRB(10, 0, 10, 0),
            margin: const EdgeInsets.fromLTRB(10, 0, 10, 0),
            child: Text(response, style: TextStyle(fontSize: 20, color: Colors.blue[1000])),
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: <Widget>[
              Container(
                padding: const EdgeInsets.all(10),
                margin: const EdgeInsets.fromLTRB(5, 10, 0, 10),
                child: FloatingActionButton.extended(
                  label: Text(boxA),
                  onPressed: () {
                    setState(() {
                      if(questionNum ==0){ //question 1
                        response = questionOne[4];
                      }
                      else if(questionNum == 1){ //question 2
                        response = questionTwo[4];
                      }
                      else if (questionNum == 2){ //question 3
                        response = questionThree[7];
                        correctAns = 3;
                        nextButton = Colors.green[300];
                      }
                      else if (questionNum == 3){ //question 4
                        response = questionFour[4];
                      }
                    });
                  },
                ),
              ),
              Container(
                padding: const EdgeInsets.all(10),
                margin: const EdgeInsets.fromLTRB(5, 10, 0, 10),
                child: FloatingActionButton.extended(
                  label: Text(boxB),
                  onPressed: () {
                    setState(() {
                      if(questionNum ==0){
                        response = questionOne[5];
                      }else if(questionNum == 1){
                        response = questionTwo[7];
                        correctAns = 2;
                        nextButton = Colors.green[300];
                      }else if (questionNum == 2){
                        response = questionThree[4];
                      }
                      else if (questionNum == 3){
                        response = questionFour[5];
                      }
                    });
                  },
                ),
              ),
            ],
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: <Widget>[
              Container(
                padding: const EdgeInsets.all(10),
                margin: const EdgeInsets.fromLTRB(5, 10, 0, 10),
                child: FloatingActionButton.extended(
                  label: Text(boxC),
                  onPressed: () {
                    setState(() {
                      if(questionNum ==0){
                        response = questionOne[7];
                        correctAns = 1;
                        nextButton = Colors.green[300];
                      }else if(questionNum == 1){
                        response = questionTwo[5];
                      }else if (questionNum == 2){
                        response = questionThree[5];
                      }
                      else if (questionNum == 3){
                        response = questionFour[6];
                      }
                    });
                  },
                ),
              ),
              Container(
                padding: const EdgeInsets.all(10),
                margin: const EdgeInsets.fromLTRB(5, 10, 0, 10),
                child: FloatingActionButton.extended(
                  label: Text(boxD),
                  onPressed: () {
                    setState(() {
                      if(questionNum ==0){
                        response = questionOne[6];
                      }else if(questionNum == 1){
                        response = questionTwo[6];
                      }else if (questionNum == 2){
                        response = questionThree[6];
                      }
                      else if (questionNum == 3){
                        response = questionFour[7];
                        correctAns = 4;
                        nextButton = Colors.green[300];
                        buttonText = 'Continue!';

                      }
                    });
                  },
                ),
              ),
            ],
          ),

          //There probably needs to be a continue button here, but will implement later as this is a quiz section
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            child: FloatingActionButton.extended(
              label: Text(buttonText, style: TextStyle(fontSize: 20, color: Colors.black)),
              backgroundColor: nextButton,
              onPressed: () {
                setState(() { //setState notifies framework that value has changed
                  if(nextButton == Colors.grey[300]){

                  }
                  else if (correctAns == 1){
                    questionNum = 1;
                    nextButton = Colors.grey[300];
                    response = '';
                    boxA = questionTwo[0];
                    boxB = questionTwo[1];
                    boxC = questionTwo[2];
                    boxD = questionTwo[3];
                  }
                  else if (correctAns == 2){
                    questionNum = 2;
                    nextButton = Colors.grey[300];
                    response = '';
                    boxA = questionThree[0];
                    boxB = questionThree[1];
                    boxC = questionThree[2];
                    boxD = questionThree[3];
                  }
                  else if (correctAns == 3){
                    questionNum = 3;
                    nextButton = Colors.grey[300];
                    response = '';
                    boxA = questionFour[0];
                    boxB = questionFour[1];
                    boxC = questionFour[2];
                    boxD = questionFour[3];
                  }
                  else if (correctAns == 4){
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => const EndOfLesson()),
                    );
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
class EndOfLesson extends StatefulWidget {
  const EndOfLesson({super.key});

  @override
  State<EndOfLesson> createState() => _EndOfLessonState();
}

class _EndOfLessonState extends State<EndOfLesson> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('End Of Lesson', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 30, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
              child: Text('This marks the end of the lesson. Please let the tester know that you have finished the lesson. Thank you!', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
            ),

          ],
        ),
      ),
    );
  }
}

//Accessory/Extra information, beyond the outline
//
//
//******************
class ComputerEffects extends StatelessWidget { //Computer Effects tab, from Outage Scenario
  const ComputerEffects({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Effects on a Computer', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 24, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
              child: Text('Electrical components have a rated voltage and current. During a surge, this value may drastically change, then decrease.', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
            ),
            Center(
                child: Image(
                  image: AssetImage('assets/surge_current.jpg'),
                  height: 250,
                  width: 400,
                )
            ),
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
              child: Text('This surge in current and voltage can have adverse effects on some devices, including computers without protection or charging phones.', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
            ),
            Center(
              child: ElevatedButton(onPressed: (){
                Navigator.pop(context);
              },
                  child: const Text('Return to Previous Page')),
            ),
          ],
        ),
      ),
    );
  }
}

class ACUnit extends StatelessWidget { //Cooling effects tab, from Outage Scenario
  const ACUnit({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Effects on an AC Unit',style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 24, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: <Widget>[
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            child: Text('The law of equilibrium, temperature changes overtime to match environment temperature.', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
          ),
          Center(
              child: Image(
                image: AssetImage('assets/AC_Unit.PNG'),
                height: 200,
                width: 200,
              )
          ),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            child: Text('In this case, assume the temperature inside your building is ... and outside is ...', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
          ),
          Center(
              child: Image(
                image: AssetImage('assets/quadratic_formula.PNG'),
                height: 100,
                width: 500,
              )
          ),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
            child: Text('In the course of an hour, the temperature change inside the building will be...', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
          ),
          Center(
            child: ElevatedButton(onPressed: (){
              Navigator.pop(context);
            },
                child: const Text('Return to Previous Page')),
          ),
        ],
      ),

    );
  }
}

class NaturalDisasters extends StatelessWidget { //natural disasters tab, from causes of outages
  const NaturalDisasters({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Natural Disasters', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 24, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Flexible(
              flex: 5,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 5),
                child: Text('Natural disasters can cause equipment failure. This is anywhere from power plants failing, lightning striking the power grid, lines falling, and more.', style: TextStyle(fontSize: 21, color: Colors.blue[1000])),
              ),
            ),
            Flexible(
              flex: 5,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 5),
                child: Text('One such example is Winter Storm Uri. The cold had caused many power plants to "trip", which meant there was not enough power generation on the grid leading to rolling outages.', style: TextStyle(fontSize: 21, color: Colors.blue[1000])),
              ),
            ),
            Flexible(
              flex: 5,
              child: Center(
                  child: Image(
                    image: AssetImage('assets/stormUri.jpg'),
                    height: 250,
                    width: 400,
                  )
              ),
            ),
            Flexible(
              flex: 5,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
                child: Text('Outages caused by these tend to be the most severe as they impact the grid the most. They can last anywhere from a few minutes to several weeks.', style: TextStyle(fontSize: 21, color: Colors.blue[1000])),
              ),
            ),
            Flexible(
              flex: 2,
              child: Center(
                child: ElevatedButton(onPressed: (){
                  Navigator.pop(context);
                },
                    child: const Text('Return to Previous Page')),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class Animals extends StatelessWidget { //animals tab, from causes of outages
  const Animals({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Animals', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 24, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Flexible(
              flex: 5,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 5),
                child: Text('Animals often use power lines for different reasons. Squirrels may use them as "highways", birds may use them as a place to perch, and more.', style: TextStyle(fontSize: 21, color: Colors.blue[1000])),
              ),
            ),
            Flexible(
              flex: 6,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                child: Text('This tends to be harmless as they are not "completing the circuit." However, when they touch another wire, they will create a "short circuit," where electricity passes through their body. This creates something called a "fault."', style: TextStyle(fontSize: 21, color: Colors.blue[1000])),
              ),
            ),
            Flexible(
              flex: 5,
              child: Center(
                  child: Image(
                    image: AssetImage('assets/grackles.jpg'),
                    height: 200,
                    width: 400,
                  )
              ),
            ),
            Flexible(
              flex: 6,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                child: Text('Outages caused by short circuits tend to last a few seconds to a minute, as current power grids have protection against these cases. In more severe cases, they may last longer.', style: TextStyle(fontSize: 21, color: Colors.blue[1000])),
              ),
            ),
            Flexible(
              flex: 2,
              child: Center(
                child: ElevatedButton(onPressed: (){
                  Navigator.pop(context);
                },
                    child: const Text('Return to Previous Page')),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class HumanError extends StatelessWidget { //human error tab, from causes of outages
  const HumanError({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Human Error', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 24, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Flexible(
              flex: 5,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                child: Text('Although we all acknowledge power lines as dangerous, accidents are always prone to happen. This is from miscalculations, car accidents, and more.', style: TextStyle(fontSize: 21, color: Colors.blue[1000])),
              ),
            ),
            Flexible(
              flex: 6,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 5),
                child: Text('Construction may lead to objects, such as trees, dropping on the lines and causing a short on the lines.', style: TextStyle(fontSize: 21, color: Colors.blue[1000])),
              ),
            ),
            Flexible(
              flex: 5,
              child: Center(
                  child: Image(
                    image: AssetImage('assets/constructionWork.jpg'),
                    height: 200,
                    width: 400,
                  )
              ),
            ),
            Flexible(
              flex: 6,
              child: Container(
                padding: const EdgeInsets.all(10.0),
                margin: const EdgeInsets.fromLTRB(10, 0, 10, 10),
                child: Text('Outages caused by short circuits tend to last a few seconds to a minute, while outages caused by dropping objects staying there may have to be done manually so may last a few days.', style: TextStyle(fontSize: 21, color: Colors.blue[1000])),
              ),
            ),
            Flexible(
              flex: 2,
              child: Center(
                child: ElevatedButton(onPressed: (){
                  Navigator.pop(context);
                },
                    child: const Text('Return to Previous Page')),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class EquipmentFailure extends StatelessWidget { //equipment failure tab, from causes of outages
  const EquipmentFailure({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Equipment Failure',style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 24, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 0, 10, 10),
              child: Text('Equipment failure can happen at any time. This may be due to wear and tear on the lines.', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
            ),
            Center(
                child: Image(
                  image: AssetImage('assets/lineEquipment.PNG'),
                  height: 250,
                  width: 400,
                )
            ),
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
              child: Text('Depending on how old a line may be, power may be redirected from another location, or may be isolated. Usually equipment replacement falls under planned outages. ', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
            ),
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
              child: Text('These outages can last anywhere from a few hours to a few days.', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
            ),
            Center(
              child: ElevatedButton(onPressed: (){
                Navigator.pop(context);
              },
                  child: const Text('Return to Previous Page')),
            ),
          ],
        ),
      ),
    );
  }
}
class PlannedOutages extends StatelessWidget { //equipment failure tab, from causes of outages
  const PlannedOutages({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Planned Outages', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 24, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 0, 10, 10),
              child: Text('Sometimes, utilities will need to turn off power flow to ensure the safety of those working on the power lines.', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
            ),
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
              child: Text('If this is the case, utilities will warn those who are in the area that their power will be turned off for some amount of time.', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
            ),
            Center(
                child: Image(
                  image: AssetImage('assets/powerOutage.PNG'),
                  height: 250,
                  width: 400,
                )
            ),
            Container(
              padding: const EdgeInsets.all(10.0),
              margin: const EdgeInsets.fromLTRB(10, 20, 10, 10),
              child: Text('These outages usually only last a few hours, but may last longer depending on if there is a problem that arises.', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
            ),
            Center(
              child: ElevatedButton(
                  onPressed: (){
                Navigator.pop(context);
              },
                  child: const Text('Return to Previous Page', style: TextStyle(color: Colors.black))),
            ),
          ],
        ),
      ),
    );
  }
}

class EmergencyKit extends StatelessWidget {
  const EmergencyKit({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Emergency Kits', style: TextStyle(fontSize: 30, color: Colors.black)),
        centerTitle: true,
        backgroundColor: Colors.white,
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: <Widget>[
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 0, 10, 10),
            child: Text('Emergency kits are meant to be used over the course of a few days (usually 72 hours).', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
          ),
          Container(
            padding: const EdgeInsets.all(10.0),
            margin: const EdgeInsets.fromLTRB(10, 20, 10, 0),
            child: Text('Inside this kit, the following should be kept:', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
          ),
          Container(
            padding: const EdgeInsets.all(0),
            margin: const EdgeInsets.fromLTRB(0, 0, 0, 0),
            child: Text('''
            \u2022 No-Cook foods
            \u2022 Water (one gallon per day)                             
            \u2022 Battery powered radios
            \u2022 Flashlights                   
            \u2022 First Aid Kits
            \u2022 Extra batteries
            \u2022 Phones with backup charging
            ''', style: TextStyle(fontSize: 22, color: Colors.blue[1000])),
          ),
          Center(
              child: Image(
                image: AssetImage('assets/emergency_kit.jpg'),
                height: 250,
                width: 400,
              )
          ),
          Center(
            child: ElevatedButton(onPressed: (){
              Navigator.pop(context);
            },
                child: const Text('Return to Previous Page')),
          ),
        ],
      ),
    );
  }
}


