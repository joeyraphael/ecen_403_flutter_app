import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:google_fonts/google_fonts.dart';
import 'middle_school_page.dart';
import 'high_school_page.dart';
import 'elementary_page.dart';


// void main() => runApp(MaterialApp(
//   title: 'Power Outage Education App',
//   home: Home(),
// ));

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await SystemChrome.setPreferredOrientations([
    DeviceOrientation.portraitUp,
    DeviceOrientation.portraitDown,
  ]);

  runApp(MaterialApp(title: 'Power Outage Education App', home: Home()));
}

class Home extends StatelessWidget { //Base page design, Home buttons
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
      body: Container(
        color: Colors.white,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Spacer(),
            Flexible(
              flex: 5,
              fit: FlexFit.loose,
              child: Center(
                child: Material(
                  color: Color(0xFF800000),
                  elevation: 8,
                  borderRadius: BorderRadius.circular(28),
                  clipBehavior: Clip.antiAliasWithSaveLayer,
                  child: InkWell(
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (context) => const ElementaryPage()),
                      );
                    },
                    child: Column(
                      children: [
                        Ink.image(
                          image: AssetImage('assets/elementaryGame.PNG'),
                          height: 170,
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
            Spacer(),
            Flexible(
              flex: 5,
              child: Center(
                child: Material(
                  color: Color(0xFF800000),
                  elevation: 8,
                  borderRadius: BorderRadius.circular(28),
                  clipBehavior: Clip.antiAliasWithSaveLayer,
                  child: InkWell(
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (context) => const MiddleSchoolPage()),
                      );
                    },
                    child: Column(
                      children: [
                        Ink.image(
                          image: AssetImage('assets/middleschoolGame.PNG'),
                          height: 170,
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
            Flexible(
              flex: 5,
              child: Center(
                child: Material(
                  color: Color(0xFF800000),
                  elevation: 8,
                  borderRadius: BorderRadius.circular(28),
                  clipBehavior: Clip.antiAliasWithSaveLayer,
                  child: InkWell(
                    onTap: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(builder: (context) => const HighSchoolPage()),
                      );
                    },
                    child: Column(
                      children: [
                        Ink.image(
                          image: AssetImage('assets/powerOutage.PNG'),
                          height: 170,
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



