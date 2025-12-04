import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:google_fonts/google_fonts.dart';
import 'middle_school_page.dart';
import 'high_school_page.dart';
import 'elementary_page.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await SystemChrome.setPreferredOrientations([ //preferred app orientation (portrait)
    DeviceOrientation.portraitUp,
    DeviceOrientation.portraitDown,
  ]);
  runApp(MaterialApp(title: 'Power Outage Education App', home: Home()));
}

class Home extends StatelessWidget { //Base page design,
  // Home buttons to navigate to each page
  // - DoSeum Game
  // - Power Tycoon
  // - High School Lesson
  const Home({super.key});
  @override
  Widget build(BuildContext context) {
    // Force Portrait for Home Screen
    SystemChrome.setPreferredOrientations([
      DeviceOrientation.portraitUp,
    ]);
    return Scaffold(
      appBar: AppBar(
        title: Text('Power Outage Education App', style: GoogleFonts.lato(textStyle: TextStyle(fontSize: 24, color: Colors.white, fontWeight: FontWeight.bold))),
        centerTitle: true,
        backgroundColor: Color(0xFF800000),
      ),
      body: Container(//background
        color: Colors.white,
        child: Column( //column of all buttons
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Spacer(),
            Flexible( //Elementary page button, buttons spread out based on how much available space
              flex: 5,
              fit: FlexFit.loose,
              child: Center(
                child: Material( //design
                  color: Color(0xFF800000),
                  elevation: 8,
                  borderRadius: BorderRadius.circular(28),
                  clipBehavior: Clip.antiAliasWithSaveLayer,
                  child: InkWell( //button function -> elementary page
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (context) => const ElementaryPage()),
                      );
                    },
                    child: Column( //picture and text
                      children: [
                        Ink.image(
                          image: AssetImage('assets/elementaryGame.PNG'),
                          height: 160,
                          width: 300,
                          fit: BoxFit.cover,
                        ),
                        SizedBox(height: 6),
                        Text('K-5 Elementary',style: TextStyle(fontSize: 18, color: Colors.white)),
                      ],
                    ),
                  ),
                ),
              ),
             ),
            Spacer(), //space between buttons
            Flexible( //Middle school button
              flex: 5,
              child: Center(
                child: Material( //design
                  color: Color(0xFF800000),
                  elevation: 8,
                  borderRadius: BorderRadius.circular(28),
                  clipBehavior: Clip.antiAliasWithSaveLayer,
                  child: InkWell( //button function -> middle school page
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (context) => const MiddleSchoolPage()),
                      );
                    },
                    child: Column( // text and picture
                      children: [
                        Ink.image(
                          image: AssetImage('assets/PowerTycoon.PNG'),
                          height: 160,
                          width: 300,
                          fit: BoxFit.cover,
                        ),
                        SizedBox(height: 6),
                        Text('6-8th Middle School',style: TextStyle(fontSize: 18, color: Colors.white)),
                      ],
                    ),
                  ),
                ),
              ),
             ),
            Spacer(),
            Flexible( //high school button
              flex: 5,
              child: Center(
                child: Material( //design
                  color: Color(0xFF800000),
                  elevation: 8,
                  borderRadius: BorderRadius.circular(28),
                  clipBehavior: Clip.antiAliasWithSaveLayer,
                  child: InkWell( //button function -> high school page
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (context) => const HighSchoolPage()),
                      );
                    },
                    child: Column( //text and image
                      children: [
                        Ink.image(
                          image: AssetImage('assets/powerOutage.PNG'),
                          height: 160,
                          width: 300,
                          fit: BoxFit.cover,
                        ),
                        SizedBox(height: 6),
                        Text('9-12th High School',style: TextStyle(fontSize: 18, color: Colors.white)),
                      ],
                    ),
                  ),
                ),
              ),
            ),
            Spacer(),
          ]
        ),
      )
    );
  }
}



