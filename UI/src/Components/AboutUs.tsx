import { Link, Stack, StackItem, Text } from "@fluentui/react";
import * as React from "react";
import ButtonAppBar from "../LoginBar";
import { genericGapStackTokensString } from "./ProductCardGrid/ProductCardGridStyles";
import { makeStyles } from "@material-ui/core/styles";
import Timeline from "@material-ui/lab/Timeline";
import TimelineItem from "@material-ui/lab/TimelineItem";
import TimelineSeparator from "@material-ui/lab/TimelineSeparator";
import TimelineConnector from "@material-ui/lab/TimelineConnector";
import TimelineContent from "@material-ui/lab/TimelineContent";
import TimelineOppositeContent from "@material-ui/lab/TimelineOppositeContent";
import TimelineDot from "@material-ui/lab/TimelineDot";
import {
  Create,
  Add,
  WatchLater,
  Payment,
  HowToVote,
  LocalShipping
} from "@material-ui/icons";
import LaptopMacIcon from "@material-ui/icons/LaptopMac";
import HotelIcon from "@material-ui/icons/Hotel";
import RepeatIcon from "@material-ui/icons/Repeat";
import Paper from "@material-ui/core/Paper";
import Typography from "@material-ui/core/Typography";

export const AboutUs: React.FunctionComponent = () => {
  const classes = useStyles();
  return (
    <Stack
      tokens={{
        childrenGap: "2rem",
        padding: 10,
      }}
    >
      <ButtonAppBar />
      <StackItem align="center">
        <Text variant={"superLarge"} styles={getStylesColour("#70706c")}>
          About us
        </Text>
      </StackItem>
      <StackItem align="center">
        <Text block variant={"xxLarge"} styles={getStylesColour("#70706c")}>
          We are not a shopping site. We are something new and different.
        </Text>
      </StackItem>
      <Stack tokens={genericGapStackTokensString("1rem")}>
        <Stack
          horizontal
          wrap
          horizontalAlign="center"
          styles={{ root: { paddingBottom: "5rem" } }}
        >
          <Stack styles={{ root: { width: "40%", marginTop: "1rem" } }}>
            <Text
              block
              variant={"xxLarge"}
              styles={{ root: { color: "#1976d2", paddingBottom: "1rem" } }}
            >
              For consumers:
            </Text>
            <Text block variant={"large"}>
              We are a platform that connects people who have the same products
              on their wish list, allows them to unite into a purchasing group,
              creates order in the supplier search process and centralizes all
              the details throughout the group purchasing process.
            </Text>
            <Text block variant={"large"}>
              In short, we’re a new alternative for you to get sweet deals on
              your desired products.
            </Text>
          </Stack>
          <Stack styles={{ root: { width: "40%" } }}>
            <Timeline align="alternate">
              <TimelineItem>
                <TimelineSeparator>
                  <TimelineDot color="primary">
                    <Add />
                  </TimelineDot>
                  <TimelineConnector />
                </TimelineSeparator>
                <TimelineContent>
                  <Paper elevation={3} className={classes.paper}>
                    <Typography variant="h6" component="h1">
                      Create Group
                    </Typography>
                  </Paper>
                </TimelineContent>
              </TimelineItem>
              <TimelineItem>
                <TimelineOppositeContent></TimelineOppositeContent>
                <TimelineSeparator>
                  <TimelineDot color="primary">
                    <Create />
                  </TimelineDot>
                  <TimelineConnector />
                </TimelineSeparator>
                <TimelineContent>
                  <Paper elevation={3} className={classes.paper}>
                    <Typography variant="h6" component="h1">
                      Fill product details
                    </Typography>
                  </Paper>
                </TimelineContent>
              </TimelineItem>
              <TimelineItem>
                <TimelineSeparator>
                  <TimelineDot color="primary">
                    <WatchLater />
                  </TimelineDot>
                  <TimelineConnector />
                </TimelineSeparator>
                <TimelineContent>
                  <Paper elevation={3} className={classes.paper}>
                    <Typography variant="h6" component="h1">
                      Wait for others to join
                    </Typography>
                  </Paper>
                </TimelineContent>
              </TimelineItem>
              <TimelineItem>
                <TimelineSeparator>
                  <TimelineDot color="primary">
                    <HowToVote />
                  </TimelineDot>
                  <TimelineConnector />
                </TimelineSeparator>
                <TimelineContent>
                  <Paper elevation={3} className={classes.paper}>
                    <Typography variant="h6" component="h1">
                      Vote for a favorite supplier
                    </Typography>
                  </Paper>
                </TimelineContent>
              </TimelineItem>
              <TimelineItem>
                <TimelineSeparator>
                  <TimelineDot color="primary">
                    <Payment />
                  </TimelineDot>
                </TimelineSeparator>
                <TimelineContent>
                  <Paper elevation={3} className={classes.paper}>
                    <Typography variant="h6" component="h1">
                      Payment
                    </Typography>
                    <Typography></Typography>
                  </Paper>
                </TimelineContent>
              </TimelineItem>
            </Timeline>
          </Stack>
        </Stack>
        <Stack horizontal wrap horizontalAlign="center">
          <Stack styles={{ root: { width: "40%", marginTop: "1rem" } }}>
            <Text
              block
              variant={"xxLarge"}
              styles={{ root: { color: "#dc004e", paddingBottom: "1rem" } }}
            >
              For suppliers:
            </Text>
            <Text block variant={"large"}>
              We unite consumers into groups who are willing to purchase in
              bulk, allow suppliers to browse these groups and make them
              proposals, and centralizes all details throughout the selling
              process in case their proposals are selected. In short, we are a
              new and convenient way for you to wholesale.
            </Text>
            <Text block variant={"large"}>
              In short, we are a new and convenient way for you to wholesale.
            </Text>
          </Stack>
          <Stack styles={{ root: { width: "40%" } }}>
            <Timeline align="alternate">
              <TimelineItem>
                <TimelineSeparator>
                  <TimelineDot color="secondary">
                    <Add />
                  </TimelineDot>
                  <TimelineConnector />
                </TimelineSeparator>
                <TimelineContent>
                  <Paper elevation={3} className={classes.paper}>
                    <Typography variant="h6" component="h1">
                      Add a new proposal
                    </Typography>
                  </Paper>
                </TimelineContent>
              </TimelineItem>
              <TimelineItem>
                <TimelineOppositeContent></TimelineOppositeContent>
                <TimelineSeparator>
                  <TimelineDot color="secondary">
                    <Create />
                  </TimelineDot>
                  <TimelineConnector />
                </TimelineSeparator>
                <TimelineContent>
                  <Paper elevation={3} className={classes.paper}>
                    <Typography variant="h6" component="h1">
                      Fill the proposal form
                    </Typography>
                  </Paper>
                </TimelineContent>
              </TimelineItem>
              <TimelineItem>
                <TimelineSeparator>
                  <TimelineDot color="secondary">
                    <WatchLater />
                  </TimelineDot>
                  <TimelineConnector />
                </TimelineSeparator>
                <TimelineContent>
                  <Paper elevation={3} className={classes.paper}>
                    <Typography variant="h6" component="h1">
                      Wait for others to join
                    </Typography>
                  </Paper>
                </TimelineContent>
              </TimelineItem>
              <TimelineItem>
                <TimelineSeparator>
                  <TimelineDot color="secondary">
                  <Payment />
                  </TimelineDot>
                  <TimelineConnector />
                </TimelineSeparator>
                <TimelineContent>
                  <Paper elevation={3} className={classes.paper}>
                    <Typography variant="h6" component="h1">
                    Manage payments
                    </Typography>
                  </Paper>
                </TimelineContent>
              </TimelineItem>
              <TimelineItem>
                <TimelineSeparator>
                  <TimelineDot color="secondary">
                    <LocalShipping />
                  </TimelineDot>
                </TimelineSeparator>
                <TimelineContent>
                  <Paper elevation={3} className={classes.paper}>
                    <Typography variant="h6" component="h1">
                      Supply the product
                    </Typography>
                    <Typography></Typography>
                  </Paper>
                </TimelineContent>
              </TimelineItem>
            </Timeline>
          </Stack>
        </Stack>
      </Stack>
      <Stack horizontalAlign="center">
        <Stack
          wrap
          tokens={genericGapStackTokensString("1rem")}
          styles={{ root: { width: "80%" } }}
        >
          <Text block variant={"xLarge"} styles={getStylesColour("#1469d9")}>
            How it all works:
          </Text>

          <Text block>
            Consumers can join existing group-buys and create new ones from
            scratch. A group-buy is characterized by a requested product and a
            maximal price. Suppliers can make proposals for the various groups
            (if they can provide the requested product for at-most the group’s
            maximal price), an offer is characterized by a price and a minimal
            quantity of units.
          </Text>
          <Text block variant={"large"} styles={getStylesColour("#70706c")}>
            When the group's last day to join ends:
          </Text>
          <Text block styles={extraMarginsStyle}>
            <li>
              If there are no relevant proposals to the group, the group closes.
            </li>
            <li>
              If there is only one relevant proposal, it is automatically
              selected, and the group is transferred to the payment stage.
            </li>
            <li>
              If there are several relevant proposals for the group, a survey is
              conducted between the group members for 48 hours, at the end of
              this time frame one proposal is selected (by a majority of votes)
              and the group is transferred to the payment stage.
            </li>
          </Text>
          <Text block variant={"large"} styles={getStylesColour("#70706c")}>
            Payment:
          </Text>
          <Text block>
            The selected proposal’s supplier provides a{" "}
            <Link href="https://www.paypal.com/us/smarthelp/article/faq3025">
              PayPal.Me{" "}
            </Link>{" "}
            link and the group members pay using PayPal, this assures security
            in payments and provides a dispute system for consumers. When the
            group is at the payment stage, the group’s page indicates the
            payment status of each participant. If not all participancts
            complete payment within 5 days, the deal is cancelled. Otherwise the
            deal is completed.
          </Text>
          <Text block variant={"large"} styles={getStylesColour("#70706c")}>
            Privacy:
          </Text>
          <Text block>
            Consumers personal details (such as address, phone number etc) are
            only visable to chosen suppliers of the group-buys they participate
            in.
          </Text>
        </Stack>
      </Stack>
    </Stack>
  );
};

const marginsStyle = { root: { marginLeft: "15rem", marginRight: "15rem" } };
const extraMarginsStyle = {
  root: { marginLeft: "2rem" },
};

const getStylesColour = (colorId: string) => {
  return {
    root: {
      color: colorId,
    },
  };
};

const useStyles = makeStyles((theme) => ({
  paper: {
    padding: "6px 16px",
  },
  secondaryTail: {
    backgroundColor: theme.palette.secondary.main,
  },
}));
